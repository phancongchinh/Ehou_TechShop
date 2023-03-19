using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechShop.Models.Entity;

namespace TechShop.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Name).HasMaxLength(64);
            builder.Property(e => e.Phone).HasMaxLength(16);
            builder.Property(e => e.Email).HasMaxLength(64);
            builder.Property(e => e.PasswordHash).HasMaxLength(64);
            
            builder.HasOne(e => e.UserRole).WithMany(e => e.Users).HasForeignKey(e => e.RoleId);
            builder.HasMany(e => e.ShoppingCartItems).WithOne(e => e.User).HasForeignKey(e => e.UserId);
            builder.HasMany(e => e.Purchases).WithOne(e => e.User).HasForeignKey(e => e.UserId);
        }
    }
}