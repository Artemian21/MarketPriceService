using MarketPriceService.BusinessLogic.Abstractions;
using MarketPriceService.BusinessLogic.Models;
using MarketPriceService.BusinessLogic.Services;
using MarketPriceService.DataAccess;
using MarketPriceService.DataAccess.Abstractions;
using MarketPriceService.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MarketPriceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<FintachartsSettings>(
    builder.Configuration.GetSection("Fintacharts"));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IInstrumentService, InstrumentService>();
builder.Services.AddScoped<IMarketAssetService, MarketAssetService>();
builder.Services.AddSingleton<IFintachartsWebSocketClient, FintachartsWebSocketClient>();

builder.Services.AddScoped<IMarketAssetRepository, MarketAssetRepository>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddHttpClient();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<MarketPriceDbContext>();
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

using (var scope = app.Services.CreateScope())
{
    var marketAssetService = scope.ServiceProvider.GetRequiredService<IMarketAssetService>();
    var webSocketClient = scope.ServiceProvider.GetRequiredService<IFintachartsWebSocketClient>();
    var instrumentService = scope.ServiceProvider.GetRequiredService<IInstrumentService>();

    var instruments = await instrumentService.GetInstrumentsAsync(25);

    var cts = new CancellationTokenSource();
    _ = webSocketClient.ConnectAndListenAsync(
        instruments,
        cts.Token
    );
}

app.Run();