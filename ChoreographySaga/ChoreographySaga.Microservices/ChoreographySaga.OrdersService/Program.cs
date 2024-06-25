using ChoreographySaga.Contracts.Requests;
using ChoreographySaga.OrdersService.Abstractions;
using ChoreographySaga.OrdersService.Configuration;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.MapPost("/api/Orders/Place",
    ([FromBody] PlaceOrderRequest request, IOrderService orderService, CancellationToken token) =>
        orderService.PlaceNewOrder(request, token));
app.MapGet("/api/Orders/{orderUuid:guid}",
    ([FromRoute(Name = "orderUuid")] Guid orderUuid, IOrderService orderService, CancellationToken token) =>
        orderService.GetOrderDetails(orderUuid, token));
app.Run();