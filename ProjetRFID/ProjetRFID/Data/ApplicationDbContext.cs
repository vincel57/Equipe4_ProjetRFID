using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjetRFID.Models;

namespace ProjetRFID.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Configure domain classes using modelBuilder here   
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Simulation>();
            modelBuilder.Entity<KNN>();
            modelBuilder.Entity<Random_Forest >();
            modelBuilder.Entity<SVM>();
            modelBuilder.Entity<Analytique>();
        }

        public DbSet<ProjetRFID.Models.KNN> KNN { get; set; }
        
       
        
        public DbSet<ProjetRFID.Models.Analytique> Analytique { get; set; }
        public DbSet<ProjetRFID.Models.Random_Forest>? Random_Forest { get; set; }
        public DbSet<ProjetRFID.Models.SVM>? SVM { get; set; }
        public DbSet<ProjetRFID.Models.Simulation>? Simulation { get; set; }

    }
}
