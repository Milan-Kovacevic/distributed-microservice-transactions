using ChoreographySaga.PaymentService.Configuration;
using ChoreographySaga.PaymentService.Services;
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
        app.ApplyMigration();
        app.ApplyDataSeed();
}

app.MapGet("/api/Payment/Clients/{userUuid:guid}",
    ([FromRoute(Name = "userUuid")] Guid userUuid, IPaymentService paymentService, CancellationToken token) =>
        paymentService.GetClientDetails(userUuid, token));
// TODO: Register rest of the endpoints for managing payment process (CRUD operations)
app.Run();