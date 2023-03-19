using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechShop.Models.Entity;

namespace TechShop.Data.Configurations
{
    public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            
            builder.HasMany(e => e.PurchaseProducts).WithOne(e => e.Purchase).HasForeignKey(e => e.PurchaseId);
            builder.HasOne(e => e.User).WithMany(e => e.Purchases).HasForeignKey(e => e.UserId);
        }
    }
}