using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TechShop.Data.Configurations;
using TechShop.Models.Entity;

namespace TechShop.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }
        private AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public static AppDbContext Init()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            var build = new DbContextOptionsBuilder<AppDbContext>();
            build.UseSqlServer(configuration.GetConnectionString("MSSqlServer"));
            return new AppDbContext(build.Options);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            Seed.SeedData(builder);

            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());
            builder.ApplyConfiguration(new PurchaseConfiguration());
            builder.ApplyConfiguration(new PurchaseProductConfiguration());
            builder.ApplyConfiguration(new ShoppingCartItemConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new ImageConfiguration());
        }
    }
}