using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechShop.Models.Entity;

namespace TechShop.Data.Configurations
{
    public class PurchaseProductConfiguration : IEntityTypeConfiguration<PurchaseProduct>
    {
        public void Configure(EntityTypeBuilder<PurchaseProduct> builder)
        {
            builder.HasKey(e => new {e.PurchaseId, e.ProductId});

            builder.HasOne(e => e.Purchase).WithMany(e => e.PurchaseProducts).HasForeignKey(e => e.PurchaseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}