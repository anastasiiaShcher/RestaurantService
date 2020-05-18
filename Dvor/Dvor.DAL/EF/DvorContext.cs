using Dvor.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dvor.DAL.EF
{
    public class DvorContext : DbContext
    {
        public DvorContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Dish> Dishes { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<DishAllergy> DishAllergies { get; set; }

        public DbSet<UserAllergy> UserAllergies { get; set; }

        public DbSet<Allergy> Allergies { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetails> OrderDetails { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DishAllergy>()
                .HasKey(dishAllergy => new { dishAllergy.DishId, dishAllergy.AllergyId });

            modelBuilder.Entity<UserAllergy>()
                .HasKey(userAllergy => new { userAllergy.AllergyId, userAllergy.UserId });

            modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = "firstDishes", Name = "First Dishes", ImageUrl = "images/first.jpg" },
            new Category { CategoryId = "secondDishes", Name = "Second Dishes", ImageUrl = "images/second.jpg" },
            new Category { CategoryId = "lunches", Name = "Lunches", ImageUrl = "images/lunch.jpg" },
            new Category { CategoryId = "Desserts", Name = "Deserts", ImageUrl = "images/desserts.jpg" },
            new Category { CategoryId = "Drinks", Name = "Drinks", ImageUrl = "images/drinks.jpg" },
            new Category { CategoryId = "Salads", Name = "Salads", ImageUrl = "images/second2.jpg" }
            );

            modelBuilder.Entity<Allergy>().HasData(
                new Allergy { AllergyId = "cereals", Name = "Cereals containing gluten", Description = "wheat, rye, spelt, barley, amelcorn, unripe spelt grain ...", Image = "~/images/wheat.svg" },
                new Allergy { AllergyId = "crustaceans", Name = "Crustaceans", Description = "crabs, lobster, crayfish, shrimp and prawn ..", Image = "~/images/crab.svg" },
                new Allergy { AllergyId = "egg", Name = "Egg", Description = "including products with egg", Image = "~/images/eggs.svg" },
                new Allergy { AllergyId = "milk", Name = "Milk", Description = "including lactose", Image = "~/images/milk.svg" },
                new Allergy { AllergyId = "lupin", Name = "Lupin", Description = "flour, lupin flakes, lupinus, lupine, lupini or lupine beans...", Image = "~/images/lupin.svg" },
                new Allergy { AllergyId = "molluscs", Name = "Molluscs", Description = "squid, snails, clams, oysters and scallops...", Image = "~/images/shell.svg" }
            );
        }
    }
}