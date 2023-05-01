using WorkLogger;
using WorkLogger.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using WorkLogger.Domain.Automapper;
using WorkLogger.Domain.Services;
using WorkLogger.Infrastructure.Database;
using WorkLogger.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential 
    // cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddDbContext<ApplicationDbContext>(options => options
        .LogTo(Console.WriteLine, (_, level) => level == LogLevel.Information)
        .UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")!));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddServerSideBlazor();

builder.Services.AddMudServices();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddScoped<IMonthDayService, MonthDayService>();

builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    // Automatic update database when exist new migration
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Migration
    if (context.Database.IsNpgsql())
    {
        context.Database.Migrate();
    }

    // Init roles
  var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

  // Create all roles
  foreach (var roleName in Consts.RolesList)
  {
      var roleExists = roleManager.FindByNameAsync(roleName).Result;
      if (roleExists == null)
      {
          roleManager.CreateAsync(new IdentityRole(roleName)).Wait();
      }
  }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCookiePolicy();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});

app.Run();
