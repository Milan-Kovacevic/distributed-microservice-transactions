using ChoreographySaga.StocksService.Configuration;

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

// TODO: Register endpoints for managing stock products (CRUD operations)
app.Run();