

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WorkLogger.Domain.Automapper;
using WorkLogger.Domain.Entities;
using WorkLogger.Infrastructure.Database;
using WorkLogger.Services;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddDataProtection();
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    //options.UseInMemoryDatabase("InMemoryDb"));
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();

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

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddScoped<IMonthDayService, MonthDayService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IEmployeeSettingsService, EmployeeSettingsService>();
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddScoped<IHolidayService, HolidayService>();
builder.Services.AddScoped<IVacationService, VacationService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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

//
// var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()!;
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowReactApp", policy =>
//     {
//         policy.WithOrigins(allowedOrigins)
//             .AllowAnyHeader()
//             .AllowAnyMethod()
//             .AllowCredentials();
//     });
// });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    // Automatic update database when exist new migration
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();