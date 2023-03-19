using System;
using Microsoft.EntityFrameworkCore;
using TechShop.Models.Entity;
using TechShop.Services;

namespace TechShop.Data
{
    public class Seed
    {
        public static void SeedData(ModelBuilder builder)
        {
            //CREATE USER ROLES
            var role1 = new UserRole() {Id = 1, Name = "Administrator"};
            var role2 = new UserRole() {Id = 2, Name = "Moderator"};
            var role3 = new UserRole() {Id = 3, Name = "SimpleUser"};

            //CREATE CATEGORIES
            var cate1 = new Category() {Id = 1, Name = "Smartphone", Description = ""};
            var cate2 = new Category() {Id = 2, Name = "Notebook" , Description = ""};
            var cate3 = new Category() {Id = 3, Name = "Tablet" , Description = ""};
            var cate4 = new Category() {Id = 4, Name = "Smartwatch" , Description = ""};

            //CREATE IMAGES
            var image1 = new Image() {Id = 1, Path = "iphonexr.jpg"};
            var image2 = new Image() {Id = 2, Path = "samsung10e.jpg"};
            var image3 = new Image() {Id = 3, Path = "macbookpro16.jpg"};
            var image4 = new Image() {Id = 4, Path = "macbookpro13.jpg"};
            var image5 = new Image() {Id = 5, Path = "lgtv.jpg"};

            //CREATE USERS
            //password = 123456
            //role = administrator
            var admin = new User()
            {
                Id = 1,
                Name = "ADMIN",
                Phone = "0396520067",
                Email = "admin@techshop.vn",
                PasswordHash = PasswordConverter.Hash("123456"),
                RoleId = 1,
            };

            //password = 123456
            //role - moderator
            var moderator = new User()
            {
                Id = 2,
                Name = "ChinhPC",
                Phone = "0396520067",
                Email = "chinhpc@techshop.vn",
                PasswordHash = PasswordConverter.Hash("123456"),
                RoleId = 2,
            };

            //pasword for the user = 123456
            //role - simple user
            var normalUser = new User()
            {
                Id = 3,
                Name = "User 01",
                Phone = "0396520067",
                Email = "user01@techshop.vn",
                PasswordHash = PasswordConverter.Hash("123456"),
                RoleId = 3,
            };


            //CREATE PRODUCTS
            var product1 = new Product()
            {
                Id = 1,
                CategoryId = 1,
                Producer = "Apple",
                Model = "iPhone 14 Pro 128GB",
                Price = 760.0M,
                Description = "Example of description about a smartphone.",
                Quantity = 1000,
                ImageId = 1,
            };
            var product2 = new Product()
            {
                Id = 2,
                CategoryId = 1,
                Producer = "Samsung",
                Model = "Galaxy Fold",
                Price = 650.00M,
                Description = "New smartphone Samsung S10e is already in sale.",
                Quantity = 1000,
                ImageId = 2,
            };
            var product3 = new Product()
            {
                Id = 3,
                CategoryId = 2,
                Producer = "Apple",
                Model = "Macbook Pro 16\"",
                Price = 2200.00M,
                Description = "New notebook from Apple is already in our store.",
                Quantity = 1000,
                ImageId = 3,
            };
            var product4 = new Product()
            {
                Id = 4,
                CategoryId = 2,
                Producer = "Apple",
                Model = "MacBook Pro 13\" Space Gray",
                Price = 1400.00M,
                Description = "New notebook from Apple is already in our store.",
                ImageId = 4,
            };
            var product5 = new Product()
            {
                Id = 5,
                CategoryId = 3,
                Producer = "LG",
                Model = "43UM7459",
                Price = 450.00M,
                Description = "New TV with high resolution screen.",
                Quantity = 1000,
                ImageId = 5,
            };
            var product6 = new Product()
            {
                Id = 6,
                CategoryId = 4,
                Producer = "Apple",
                Model = "iPad Pro",
                Price = 450.00M,
                Description = "New tablet with 120hz screen.",
                Quantity = 1000,
                ImageId = 5,
            };

            // builder.Entity<UserRole>().HasData(role1, role2, role3);
            // builder.Entity<Image>().HasData(image1, image2, image3, image4, image5);
            // builder.Entity<Category>().HasData(cate1, cate2, cate3, cate4);
            builder.Entity<User>().HasData(admin, moderator, normalUser);
            // builder.Entity<Product>().HasData(product1, product2, product3, product4, product5);
        }
    }
}