using MarketPriceService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPriceService.DataAccess.Configurations
{
    public class InstrumentEntityConfiguration : IEntityTypeConfiguration<InstrumentEntity>
    {
        public void Configure(EntityTypeBuilder<InstrumentEntity> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.InstrumentId).IsRequired();
            builder.Property(i => i.Symbol).IsRequired();
            builder.Property(i => i.Kind);
            builder.Property(i => i.Exchange);
            builder.Property(i => i.Description);
            builder.Property(i => i.TickSize);
            builder.Property(i => i.Currency);

            builder.HasMany(i => i.Mappings)
                   .WithOne(m => m.Instrument)
                   .HasForeignKey(m => m.InstrumentEntityId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(i => i.Profile)
                   .WithOne(p => p.Instrument)
                   .HasForeignKey<InstrumentProfileEntity>(p => p.InstrumentEntityId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(ma => ma.LastPrice)
                   .HasColumnType("decimal(18,6)");

            builder.Property(ma => ma.LastUpdatedAt)
                   .HasColumnType("datetime2");
        }
    }
}
