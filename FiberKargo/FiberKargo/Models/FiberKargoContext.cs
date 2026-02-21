using FiberKargo.Models;
using System.Collections.Generic;
using System.Data.Entity;

namespace FiberKargo.Models
{
    public class FiberKargoContext : DbContext
    {
        public FiberKargoContext() : base("FiberKargoContext")
        {
            // Model değiştiğinde veritabanını yeniden oluşturur
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<FiberKargoContext>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Branch> Branches { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
