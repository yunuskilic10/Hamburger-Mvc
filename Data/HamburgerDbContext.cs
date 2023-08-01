using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCProjectHamburger.Models.Entities.Concrete;
using System.Reflection.Emit;
using MVCProjectHamburger.Models.ViewModels;

namespace MVCProjectHamburger.Data
{
    public class HamburgerDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public HamburgerDbContext(DbContextOptions<HamburgerDbContext> options)
            : base(options)
        {
        }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<ExtraIngredient> ExtraIngredients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ExtraIngredientOrder> ExtraIngredientOrders { get; set; }

        public DbSet<MenuOrder> MenuOrders { get; set; }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<AppRole> Roles { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ExtraIngredient>().HasKey(x => x.ID);
            builder.Entity<Order>().HasKey(x => x.ID);
            builder.Entity<Menu>().HasKey(x => x.ID);
            builder.Entity<ExtraIngredientOrder>().HasKey(x => x.ID);
            builder.Entity<ShoppingCart>().HasKey(x => x.ID);

            builder.Entity<MenuOrder>().HasKey(x => x.ID);


            builder.Entity<ShoppingCart>()
                                        .HasOne(s => s.ExtraIngredientOrder)
                                        .WithMany(ei => ei.ShoppingCarts)
                                        .HasForeignKey(s => s.ExtraIngredientOrderId);

            builder.Entity<ShoppingCart>()
                                       .HasOne(s => s.MenuOrder)
                                       .WithMany(m => m.ShoppingCarts)
                                       .HasForeignKey(s => s.MenuOrderId)
                                       .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.Cascade, depending on your needs

            builder.Entity<ShoppingCart>()
                                        .HasOne(s => s.ExtraIngredientOrder)
                                        .WithMany(e => e.ShoppingCarts)
                                        .HasForeignKey(s => s.ExtraIngredientOrderId)
                                        .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.Cascade, depending on your needs


            builder.Entity<AppRole>().HasData(
                new AppRole { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                new AppRole { Id = 2, Name = "User", NormalizedName = "USER" }
                );

           
            base.OnModelCreating(builder);
        }
        public DbSet<MVCProjectHamburger.Models.ViewModels.ShoppingCartVM>? ShoppingCartVM { get; set; }
    }
}
