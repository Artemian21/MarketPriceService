using MarketPriceService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPriceService.DataAccess.Configurations
{
    public class GicsInfoEntityConfiguration : IEntityTypeConfiguration<GicsInfoEntity>
    {
        public void Configure(EntityTypeBuilder<GicsInfoEntity> builder)
        {
            builder.HasKey(g => g.Id);
            builder.Property(g => g.SectorId).IsRequired();
            builder.Property(g => g.IndustryGroupId).IsRequired();
            builder.Property(g => g.IndustryId).IsRequired();
            builder.Property(g => g.SubIndustryId).IsRequired();
        }
    }
}
