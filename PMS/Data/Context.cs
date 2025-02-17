using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PMS.Common.Data.Enums;
using PMS.Models;

namespace PMS.Data;
public class Context : DbContext
{
    public Context(DbContextOptions options) : base(options)
    {
    }

    // Users Management
    public DbSet<User> Users { get; set; }
    public DbSet<SprintItem> SprintItems { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Project> Projects { get; set; }
    
    public DbSet<UserAssignedProject> UserAssignedProjects { get; set; }
    public DbSet<UserSprintItem> UserSprintItems { get; set; }
    public DbSet<UserFeature> UserFeatures { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        SeedData(modelBuilder);
        modelBuilder.Entity<UserAssignedProject>()
            .HasOne(uap => uap.User)
            .WithMany(u => u.UserAssignedProjects)
            .HasForeignKey(u => u.UserID)
            .OnDelete(DeleteBehavior.NoAction);  // Avoid cascade delete

        // Project to UserAssignedProjects
        modelBuilder.Entity<UserAssignedProject>()
            .HasOne(uap => uap.Project)
            .WithMany(p => p.UserAssignedProjects)
            .HasForeignKey(uap => uap.ProjectID)
            .OnDelete(DeleteBehavior.NoAction);

        // User to CreatedProjects
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Creator)
            .WithMany(u => u.CreatedProjects)
            .HasForeignKey(p => p.CreatorID)
            .OnDelete(DeleteBehavior.NoAction);  // Avoid cascade delete
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed roles
        modelBuilder.Entity<Role>().HasData(
            new Role { ID = 1, Name = "Admin", Description = "Administrator Role" },
            new Role { ID = 2, Name = "User", Description = "Standard User Role" }
        );

        PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
        var password = passwordHasher.HashPassword(null, "Admin123");
        modelBuilder.Entity<User>().HasData(
            new User
            {
                ID = 1, // Make sure the ID is set explicitly, as auto-generation might conflict.
                Email = "upskillingfinalproject@gmail.com",
                Password = password, // Ensure to hash passwords properly in your real app!
                Name = "Admin User",
                PhoneNo = "1234567890",
                Country = "CountryName",
                IsActive = true,
                TwoFactorAuthEnabled = false,
                IsEmailConfirmed = true,
                RoleID = 1 // Admin role
            }
        );
        
        // Seed User Features for Admin
        var features = Enum.GetValues(typeof(Feature)).Cast<Feature>().ToList();
        var userFeatures = new List<UserFeature>();

        int idCounter = 1;
        foreach (var feature in features)
        {
            userFeatures.Add(new UserFeature
            {
                ID = idCounter++,
                UserID = 1,
                Feature = feature
            });
        }

        modelBuilder.Entity<UserFeature>().HasData(userFeatures);
    }
}
