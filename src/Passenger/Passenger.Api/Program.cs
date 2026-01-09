using Passenger.Api.Endpoints;
using Passenger.Application;
using Passenger.Infrastructure;
using Passenger.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// ----------------------
// Add services
// ----------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Application Services (includes MediatR)
builder.Services.AddPassengerApplication();

// Add Infrastructure Services (Database, Repositories, etc.)
builder.Services.AddPassengerInfrastructure(builder.Configuration);

// Add Authentication & Authorization


// Add Authentication & Authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Keycloak:Authority"];
        options.Audience = builder.Configuration["Keycloak:Audience"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization();

// ----------------------
// Build the app
// ----------------------
var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PassengerDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

// ----------------------
// Middleware
// ----------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

// ----------------------
// Map endpoints
// ----------------------
app.MapPassengerEndpoints();

// ----------------------
// Run the app
// ----------------------
app.Run();
