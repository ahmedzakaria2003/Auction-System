using Auction_System;
using Auction_System.CustomMiddleWare;
using AuctionSystem.Application;
using AuctionSystem.Application.Contracts;
using AuctionSystem.Infrastructure;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
    logging.SetMinimumLevel(LogLevel.Debug);
});

// Controllers
builder.Services.AddControllers();

// Layered Service Registration
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.WithOrigins("http://localhost:4200") // Angular dev server
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});




var app = builder.Build();

// Seeder
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var seeder = services.GetRequiredService<IDataSeeding>();
    await seeder.SeedDataAsync();
}

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CustomExceptionHandlerMiddleware>(); 
app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.UseWebServices();

app.MapControllers();

app.Run();
