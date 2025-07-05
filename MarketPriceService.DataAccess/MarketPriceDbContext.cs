using MarketPriceService.DataAccess.Configurations;
using MarketPriceService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketPriceService.DataAccess
{
    public class MarketPriceDbContext : DbContext
    {
        public MarketPriceDbContext(DbContextOptions<MarketPriceDbContext> options)
            : base(options)
        {
        }

        public DbSet<InstrumentEntity> Instruments { get; set; }
        public DbSet<InstrumentMappingEntity> InstrumentMappings { get; set; }
        public DbSet<InstrumentProfileEntity> InstrumentProfiles { get; set; }
        public DbSet<GicsInfoEntity> GicsInfos { get; set; }
        public DbSet<TradingHoursEntity> TradingHours { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new InstrumentEntityConfiguration());
            modelBuilder.ApplyConfiguration(new InstrumentMappingEntityConfiguration());
            modelBuilder.ApplyConfiguration(new InstrumentProfileEntityConfiguration());
            modelBuilder.ApplyConfiguration(new GicsInfoEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TradingHoursEntityConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
