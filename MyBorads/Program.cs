using Microsoft.EntityFrameworkCore;
using MyBorads.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MyBoardsContext>(
    option => option.UseSqlServer(builder.Configuration.GetConnectionString("MyBoardsConnectionString"))
    
    );

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//jesli nie ma bazy danych lub sa zalegle migracje to taka baza zostanie utworzona tym kodem :
using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetService<DbContext>();

var pendingMigrations = dbContext.Database.GetPendingMigrations();

if (pendingMigrations.Any())
{
    dbContext.Database.Migrate();
}

app.Run();

