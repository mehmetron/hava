using hava.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace hava.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        
        public DbSet<Job> Jobs { get; set; }
        
        public DbSet<Client> Clients { get; set; }
        
        public DbSet<UserJob> UserJobs { get; set; }
        
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
                
            modelBuilder.Entity<Client>()
                .HasMany<Job>(h => h.Jobs)
                .WithOne(z => z.Client)
                .HasForeignKey(z => z.ClientId);
            
            // modelBuilder.Entity<Zone>()
            //     .HasMany<Job>(h => h.Devices)
            //     .WithOne(z => z.Zone)
            //     .HasForeignKey(z => z.ZoneId);
            
            modelBuilder.Entity<ApplicationUser>()
                .HasMany<UserJob>(h => h.UserJobs)
                .WithOne(z => z.ApplicationUser)
                .HasForeignKey(z => z.ApplicationUserId);
            
            
            modelBuilder.Entity<UserJob>()
                .HasKey(uj => new { uj.ApplicationUserId, uj.JobId });

            modelBuilder.Entity<UserJob>()
                .HasOne(uj => uj.ApplicationUser)
                .WithMany(u => u.UserJobs)
                .HasForeignKey(uj => uj.ApplicationUserId);

            modelBuilder.Entity<UserJob>()
                .HasOne(uj => uj.Job)
                .WithMany(j => j.UserJobs)
                .HasForeignKey(uj => uj.JobId);

            modelBuilder.Entity<Job>()
                .HasOne(j => j.Client)
                .WithMany(c => c.Jobs)
                .HasForeignKey(j => j.ClientId);
            
        }
        
        public override int SaveChanges()  
        {  
            ChangeTracker.DetectChanges();  
            return base.SaveChanges();  
        }  
        
    }
}