using MarketPriceService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPriceService.DataAccess.Configurations
{
    public class InstrumentProfileEntityConfiguration : IEntityTypeConfiguration<InstrumentProfileEntity>
    {
        public void Configure(EntityTypeBuilder<InstrumentProfileEntity> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasMaxLength(200);
            builder.Property(p => p.Location).HasMaxLength(200);

            builder.HasOne(p => p.Gics)
                   .WithOne(g => g.Profile)
                   .HasForeignKey<GicsInfoEntity>(g => g.InstrumentProfileEntityId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
