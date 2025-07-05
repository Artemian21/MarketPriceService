using MarketPriceService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPriceService.DataAccess.Configurations
{
    public class TradingHoursEntityConfiguration : IEntityTypeConfiguration<TradingHoursEntity>
    {
        public void Configure(EntityTypeBuilder<TradingHoursEntity> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.RegularStart).HasMaxLength(20);
            builder.Property(t => t.RegularEnd).HasMaxLength(20);
            builder.Property(t => t.ElectronicStart).HasMaxLength(20);
            builder.Property(t => t.ElectronicEnd).HasMaxLength(20);
        }
    }
}
