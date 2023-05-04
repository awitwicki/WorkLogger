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
        public DbSet<EmployeeSettings> EmployeeSettings { get; set; }
        public DbSet<Holiday> Holidays { get; set; }

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

            modelBuilder
                .Entity<Holiday>()
                .HasKey(x => x.DateDay);
        }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
    }
}
