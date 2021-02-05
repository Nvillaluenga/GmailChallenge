using Microsoft.EntityFrameworkCore;

namespace GmailChallenge.Repository
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        
        //In case we don't want to have repeated values, I don't have context on the task as to check
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Email>()
        //        .HasKey(e => new { e.Subject, e.From, e.Fecha });
        //}

        public DbSet<Email> Emails { get; set; }
    }
}
