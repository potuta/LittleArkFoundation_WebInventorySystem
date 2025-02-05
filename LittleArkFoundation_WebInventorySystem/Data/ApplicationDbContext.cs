using Microsoft.EntityFrameworkCore;
using LittleArkFoundation_WebInventorySystem.Models;

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
        public DbSet<BloodInventoryModel> BloodInventory { get; set; }
        public DbSet<BloodRequestsModel> BloodRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define primary key for Users entity
            modelBuilder.Entity<UsersModel>()
                .HasKey(u => u.UserID); // Assuming 'Id' is the primary key property

            modelBuilder.Entity<UsersModel>()
                .Property(u => u.UserID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<BloodInventoryModel>()
                .HasKey(b => b.InventoryID);

            modelBuilder.Entity<BloodRequestsModel>()
                .HasKey(h => h.RequestID);
        }
    }
}