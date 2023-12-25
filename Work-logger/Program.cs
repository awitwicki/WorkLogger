using Microsoft.AspNetCore.Identity;
using WorkLogger.Data;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using WorkLogger;
using WorkLogger.Common;
using WorkLogger.Domain.Automapper;
using WorkLogger.Domain.ConfigModels;
using WorkLogger.Infrastructure.Database;
using WorkLogger.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(builder =>
{
    builder.AddConsole();
});

builder.Services.AddDbContext<ApplicationDbContext>(options => options
         .UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")!));

builder.Services.AddTransient<ApplicationDbContext>();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
        options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = Environment.GetEnvironmentVariable("GOOGLE_OAUTH_CLIENT_ID")!;
        options.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_OAUTH_CLIENT_SECRET")!;
    });

builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

var config = new ConfigModel();
if (config.HasErrors)
{
    foreach (var errorMessage in config.ErrorMessages)
    {
        Console.WriteLine(errorMessage);
    }

    await Task.Delay(-1);
}

builder.Services.AddSingleton<ConfigModel>(x => config);

builder.Services.AddScoped<IMonthDayService, MonthDayService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IEmployeeSettingsService, EmployeeSettingsService>();
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddScoped<IHolidayService, HolidayService>();
builder.Services.AddScoped<IVacationService, VacationService>();

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
    
    // Init Holidays
    if (!context.Holidays.Any())
    {
        var holidaysToAdd = HolidaysHelpers.GetHolidays2023();
        
        context.Holidays.AddRange(holidaysToAdd);
        context.SaveChanges();
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

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
