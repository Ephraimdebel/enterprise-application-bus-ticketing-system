using Dispute.Application;
using Dispute.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

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

var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DisputeDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/disputes", async (OpenDisputeCommand command, ISender sender) =>
{
    var disputeId = await sender.Send(command);
    return Results.CreatedAtRoute("GetDispute", new { id = disputeId }, disputeId);
}).RequireAuthorization();

app.MapGet("/disputes/user/{userId}", async (Guid userId, ISender sender) =>
{
    var query = new GetUserDisputesQuery(userId);
    var response = await sender.Send(query);
    return Results.Ok(response);
}).RequireAuthorization();

app.MapGet("/disputes/{id}", async (Guid id, ISender sender) =>
{
    var query = new GetDisputeByIdQuery(id);
    var response = await sender.Send(query);
    return response is not null ? Results.Ok(response) : Results.NotFound();
}).WithName("GetDispute").RequireAuthorization();

app.MapPost("/disputes/{id}/messages", async (Guid id, AddDisputeMessageCommand command, ISender sender) =>
{
    if (id != command.DisputeId) return Results.BadRequest("ID mismatch");
    await sender.Send(command);
    return Results.NoContent();
}).RequireAuthorization();

app.MapPut("/disputes/{id}/status", async (Guid id, ChangeDisputeStatusCommand command, ISender sender) =>
{
    if (id != command.DisputeId) return Results.BadRequest("ID mismatch");
    await sender.Send(command);
    return Results.NoContent();
}).RequireAuthorization();

app.Run();
