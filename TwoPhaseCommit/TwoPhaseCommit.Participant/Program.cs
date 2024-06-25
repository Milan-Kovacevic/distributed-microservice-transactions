using Microsoft.EntityFrameworkCore;
using TwoPhaseCommit.Participant;
using TwoPhaseCommit.Participant.Abstractions;
using TwoPhaseCommit.Participant.Persistence;
using TwoPhaseCommit.Participant.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options
    => options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddTransient<IDataManager, DataManager>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();