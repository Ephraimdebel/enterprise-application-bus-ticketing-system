using Passenger.Api.Endpoints;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// ----------------------
// Add services
// ----------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add MediatR
builder.Services.AddMediatR(typeof(Program).Assembly);


// Add Authentication & Authorization
builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        // TODO: Replace with your real auth server URL
        options.Authority = "https://your-auth-server";
        options.TokenValidationParameters.ValidateAudience = false;
    });

// ----------------------
// Build the app
// ----------------------
var app = builder.Build();

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
