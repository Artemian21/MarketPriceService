using MarketPriceService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPriceService.DataAccess.Configurations
{
    public class InstrumentMappingEntityConfiguration : IEntityTypeConfiguration<InstrumentMappingEntity>
    {
        public void Configure(EntityTypeBuilder<InstrumentMappingEntity> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Symbol).IsRequired().HasMaxLength(100);
            builder.Property(m => m.Exchange).HasMaxLength(50);
            builder.Property(m => m.DefaultOrderSize).IsRequired();
            builder.Property(m => m.MaxOrderSize);

            builder.HasOne(m => m.TradingHours)
                   .WithOne()
                   .HasForeignKey<TradingHoursEntity>(t => t.Id)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
