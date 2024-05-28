namespace Онлайн_билеты.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.Diagnostics.Metrics;

    public class BiletsContext : DbContext
    {
        public DbSet<Tickets> Ticket { get; set; }
        public DbSet<Korzina> Korzinas { get; set; }
        public DbSet<Otkuda> Otkudas { get; set; }
        public DbSet<Kuda> Kudas { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Check> Checks { get; set; }
        public DbSet<Prodano> Prodanos { get; set; }
        

        public BiletsContext(DbContextOptions<BiletsContext> options)
               : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
