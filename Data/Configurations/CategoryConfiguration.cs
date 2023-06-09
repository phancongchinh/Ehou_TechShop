﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechShop.Models.Entity;

namespace TechShop.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Description).HasMaxLength(128);
            builder.Property(e => e.Name).HasMaxLength(32);
            builder.HasMany(e => e.Products).WithOne(e => e.Category).HasForeignKey(e => e.CategoryId);
        }
    }
}