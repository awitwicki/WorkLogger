using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WorkLogger.Domain.Entities;

namespace WorkLogger.Infrastructure.Database
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<MonthWorkDay> MonthWorkDays { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder
                .Entity<MonthWorkDay>()
                .Property(e => e.Days)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<MonthWorkDayItem[]>(v)!);

            modelBuilder
                .Entity<IdentityUser>()
                .HasKey(x => x.Id);
        }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
    }
}
