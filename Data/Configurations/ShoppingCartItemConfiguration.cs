using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechShop.Models.Entity;

namespace TechShop.Data.Configurations
{
    public class ShoppingCartItemConfiguration : IEntityTypeConfiguration<ShoppingCartItem>
    {
        public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
        {
            builder.HasKey(e => new {e.UserId, e.ProductId});
            builder.HasOne(e => e.User).WithMany(e => e.ShoppingCartItems).HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.Product).WithMany(e => e.ShoppingCarts).HasForeignKey(e => e.ProductId);
        }
    }
}