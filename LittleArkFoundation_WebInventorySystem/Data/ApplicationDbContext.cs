using Microsoft.EntityFrameworkCore;
using LittleArkFoundation_WebInventorySystem.Areas.Admin.Models;

namespace LittleArkFoundation_WebInventorySystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly string _connectionString;

        public ApplicationDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        public DbSet<UsersModel> Users { get; set; } 
        public DbSet<UsersArchivesModel> UsersArchives { get; set; }
        public DbSet<RolesModel> Roles { get; set; }
        public DbSet<RolePermissionsModel> RolePermissions { get; set; }
        public DbSet<PermissionsModel> Permissions { get; set; }
        public DbSet<BloodInventoryModel> BloodInventory { get; set; }
        public DbSet<BloodRequestsModel> BloodRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Users & UsersArchives
            // Define primary key for Users entity
            modelBuilder.Entity<UsersModel>()
                .ToTable("Users")
                .HasKey(u => u.UserID); // Assuming 'Id' is the primary key property

            modelBuilder.Entity<UsersArchivesModel>()
                .ToTable("Users_Archives")
                .HasKey(u => u.UserID);

            // Roles 
            modelBuilder.Entity<RolesModel>()
                .ToTable("Roles")
                .HasKey(r => r.RoleID);

            // RolesPermissions
            modelBuilder.Entity<RolePermissionsModel>()
                .ToTable("RolePermissions")
                .HasKey(rp => rp.Id);

            modelBuilder.Entity<RolePermissionsModel>()
                .Property(rp => rp.Id)
                .ValueGeneratedOnAdd();

            // Permissions
            modelBuilder.Entity<PermissionsModel>()
                .ToTable("Permissions")
                .HasKey(p => p.PermissionID);

            modelBuilder.Entity<PermissionsModel>()
                .Property(p => p.PermissionID)
                .ValueGeneratedOnAdd();
                
            // BloodInventory & BloodInventoryArchives
            modelBuilder.Entity<BloodInventoryModel>()
                .HasKey(b => b.InventoryID);

            // BloodRequests & BloodRequestsArchives
            modelBuilder.Entity<BloodRequestsModel>()
                .HasKey(h => h.RequestID);
        }
    }
}