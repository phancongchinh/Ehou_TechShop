using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechShop.Models.Entity;

namespace TechShop.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Producer).HasMaxLength(64);
            builder.Property(e => e.Model).HasMaxLength(64);
            builder.Property(e => e.Price).HasColumnType("decimal(7,2)");
            builder.Property(e => e.Description).HasColumnType("ntext");
            builder.HasOne(e => e.Category).WithMany(e => e.Products).HasForeignKey(e => e.CategoryId);
            builder.HasMany(e => e.PurchaseProducts).WithOne(e => e.Product).HasForeignKey(e => e.ProductId);
            builder.HasMany(e => e.ShoppingCarts).WithOne(e => e.Product).HasForeignKey(e => e.ProductId);
        }
    }
}