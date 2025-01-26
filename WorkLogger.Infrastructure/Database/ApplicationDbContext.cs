using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WorkLogger.Domain.Entities;

namespace WorkLogger.Infrastructure.Database;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public DbSet<MonthWorkDay> MonthWorkDays { get; set; }
    public DbSet<EmployeeSettings> EmployeeSettings { get; set; }
    public DbSet<Holiday> Holidays { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder
            .Entity<MonthWorkDay>()
            .Property(e => e.Days)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<MonthWorkDayItem[]>(v)!);

        builder
            .Entity<ApplicationUser>()
            .HasKey(x => x.Id);

        builder
            .Entity<Holiday>()
            .HasKey(x => x.DateDay);
        
        var adminRoleId = Guid.NewGuid();
        var moderatorRoleId = Guid.NewGuid();
        var userRoleId = Guid.NewGuid();
        
        ApplicationRole adminRole = new()
        {
            Id = adminRoleId,
            Name = ApplicationRoles.Admin,
            NormalizedName = ApplicationRoles.Admin.ToUpper(),
        };

        ApplicationRole moderatorRole = new()
        {
            Id = moderatorRoleId,
            Name = ApplicationRoles.Moderator,
            NormalizedName = ApplicationRoles.Moderator.ToUpper(),
        };
        
        ApplicationRole userRole = new()
        {
            Id = userRoleId,
            Name = ApplicationRoles.UserRole,
            NormalizedName = ApplicationRoles.UserRole.ToUpper(),
        };

        builder.Entity<ApplicationRole>()
            .HasData(userRole);
        builder.Entity<ApplicationRole>()
            .HasData(moderatorRole);
        builder.Entity<ApplicationRole>()
            .HasData(adminRole);
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }
}
