using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using IonFiltra.BagFilters.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using IonFiltra.BagFilters.Infrastructure;
using Microsoft.OpenApi.Models;

try
{
    var builder = WebApplication.CreateBuilder(args);
    

    // Add Infrastructure (DbContext + repositories)
    builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("DefaultConnection"));

    // Configure JWT authentication
    var jwtKey = builder.Configuration["Jwt:Key"];
    var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
            };
        });

    // Add Controllers
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    // Add Swagger with OpenAPI Info + JWT support
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "IonFiltra BagFilters API",
            Version = "v1",
            Description = "API documentation for IonFiltra BagFilters"
        });

        // ✅ Add JWT support in Swagger
        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme."
        };

        c.AddSecurityDefinition("Bearer", securityScheme);

        var securityRequirement = new OpenApiSecurityRequirement
        {
            { securityScheme, new[] { "Bearer" } }
        };

        c.AddSecurityRequirement(securityRequirement);
    });

    // Enable CORS for React
    var allowedOrigins = new[]
    {
        "http://localhost:3006"
    };

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowReactApp",
            policy => policy.WithOrigins(allowedOrigins)
                            .AllowAnyHeader()
                            .AllowAnyMethod());
    });

    var app = builder.Build();

    Console.WriteLine("Application started successfully");

    app.UseCors("AllowReactApp");

    // Swagger middleware
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "IonFiltra BagFilters API v1");
        c.RoutePrefix = "swagger"; // Open at /swagger/index.html
    });

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("Application start-up failed: " + ex.Message);
    Console.WriteLine(ex.StackTrace);
}
