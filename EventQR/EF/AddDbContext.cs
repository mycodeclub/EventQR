using EventQR.Models;
using EventQR.Models.Acc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace EventQR.EF
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.SeedRoles();

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasKey(p => new { p.UserId, p.RoleId });
            modelBuilder.Entity<AppUser>().ToTable("AppUser");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) { }
        }


        public DbSet<Event> Events { get; set; }
        public DbSet<EventGuest> Guests { get; set; } 
//        public DbSet<EventMeal> EventMeals { get; set; }
  //      public DbSet<GuestMeals> GuestMeals { get; set; }
        public DbSet<Organizer> EventOrganizers { get; set; }

        public DbSet<Inquery> Inqueries { get; set; }
    }
}
