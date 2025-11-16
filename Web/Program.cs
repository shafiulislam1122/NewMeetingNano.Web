// Program.cs

using Application.Services;
using Domain.Interfaces;
using FluentValidation.AspNetCore;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// 1️⃣ Controllers + FluentValidation
// -----------------------------
builder.Services.AddControllers()
       .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

// -----------------------------
// 2️⃣ Dapper Repositories DI
// -----------------------------
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IIUserRepository, UserRepository>();
builder.Services.AddScoped<IMeetingRoomRepository, MeetingRoomRepository>();

// -----------------------------
// 3️⃣ AuthService DI
// -----------------------------
builder.Services.AddScoped<IAuthService, AuthService>();

// -----------------------------
// 4️⃣ AppDbContext DI (EF Core for Notifications)
// -----------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// -----------------------------
// 5️⃣ Notification Repository DI
// -----------------------------
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

// -----------------------------
// 6️⃣ JWT Authentication
// -----------------------------
var jwtSettingsSection = builder.Configuration.GetSection("Jwt");
if (!jwtSettingsSection.Exists())
    throw new Exception("JWT section is missing in appsettings.json");

var jwtKey = jwtSettingsSection.GetValue<string>("Key");
if (string.IsNullOrWhiteSpace(jwtKey))
    throw new Exception("JWT Key is missing in appsettings.json");

var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = jwtSettingsSection.GetValue<string>("Issuer"),
        ValidAudience = jwtSettingsSection.GetValue<string>("Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// -----------------------------
// 7️⃣ Swagger
// -----------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NewMeetingNano API", Version = "v1" });

    // JWT Auth in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

// -----------------------------
// 8️⃣ CORS
// -----------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// -----------------------------
// 9️⃣ Middleware
// -----------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NewMeetingNano API V1");
        c.RoutePrefix = "swagger"; // Swagger URL: https://localhost:PORT/swagger/index.html
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
