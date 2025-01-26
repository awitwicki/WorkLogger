using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using WorkLogger.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
//
// builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
//         options.SignIn.RequireConfirmedAccount = false)
//     .AddRoles<IdentityRole>()
//     .AddEntityFrameworkStores<ApplicationDbContext>();

//
// builder.Services.AddIdentityCore<IdentityRole>()
//     .AddRoles<IdentityRole>()
//     .AddEntityFrameworkStores<AppDbContext>()
//     .AddApiEndpoints();


builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}
    )
    .AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    })
    .AddBearerToken(IdentityConstants.BearerScheme);


builder.Services.AddAuthorization();
builder.Services.AddAuthorizationBuilder();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddServerSideBlazor();
//builder.Services.AddMudServices();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

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
        await context.Database.MigrateAsync();
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();

// app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
   // endpoints.MapBlazorHub();
   // endpoints.MapFallbackToPage("/_Host");
});

app.Run();
