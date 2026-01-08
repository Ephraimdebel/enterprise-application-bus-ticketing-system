using Booking.Application;
using Microsoft.OpenApi.Models;
using global::Booking.Domain;
using Booking.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// builder.Services.AddOpenApi();
// Add OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Booking API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token like: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference 
                { 
                    Type = ReferenceType.SecurityScheme, 
                    Id = "Bearer" 
                }
            },
            new string[] {}
        }
    });
});


builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Keycloak:Authority"];
        options.Audience = builder.Configuration["Keycloak:Audience"];
        options.RequireHttpsMetadata = false; // For development
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking API V1");
        c.RoutePrefix = string.Empty; // Swagger UI available at root /
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/bookings", async (CreateBookingCommand command, ISender sender) =>
{
    var bookingId = await sender.Send(command);
    return Results.CreatedAtRoute("GetBooking", new { id = bookingId }, bookingId);
}).RequireAuthorization();

app.MapGet("/bookings/{id}", async (Guid id, ISender sender) =>
{
    var query = new GetBookingQuery(id);
    var response = await sender.Send(query);
    return Results.Ok(response);
}).WithName("GetBooking").RequireAuthorization();

app.MapPut("/bookings/{id}/confirm", async (Guid id, ISender sender) =>
{
    await sender.Send(new ConfirmBookingCommand(id));
    return Results.NoContent();
}).RequireAuthorization();

app.MapPut("/bookings/{id}/cancel", async (Guid id, ISender sender) =>
{
    await sender.Send(new CancelBookingCommand(id));
    return Results.NoContent();
}).RequireAuthorization();

app.Run();
