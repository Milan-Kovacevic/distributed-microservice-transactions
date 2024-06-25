using FlightsService.Abstractions;
using FlightsService.Configuration;
using Microsoft.AspNetCore.Mvc;
using TransactionalOutbox.Contracts.Requests;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigration();
    app.ApplyDataSeed();
}

app.MapPost("/api/Flights",
    ([FromBody] BookFlightRequest request, IFlightService flightService) =>
        flightService.BookAFlight(request));
app.MapGet("/api/Flights",
    (IFlightService flightService) => flightService.GetAllFlights());
app.MapGet("/api/Flights/Bookings",
    (IFlightService flightService) => flightService.GetAllBookedFlights());
app.Run();