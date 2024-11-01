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
var dbContext = scope.ServiceProvider.GetService<MyBoardsContext>();

var pendingMigrations = dbContext.Database.GetPendingMigrations();

if (pendingMigrations.Any())
{
    dbContext.Database.Migrate();
}

var users = dbContext.Users.ToList();
if (!users.Any())
{
    var user1 = new User()
    {
        Email = "user1@test.com",
        FullName = "User one",
        Adress = new Address()
        {
            City = "Warszawa",
            Street = "Szeroka"
        }
    };
    var user2 = new User()
    {
        Email = "user2@test.com",
        FullName = "User two",
        Adress = new Address()
        {
            City = "Krakow",
            Street = "Dluga"
        }
    };
    dbContext.Users.AddRange(user1, user2);
    dbContext.SaveChanges();
}

//Metoda GetPendingMigrations() jest czêœci¹ API migracji Entity Framework Core. S³u¿y do uzyskania listy migracji, //które zosta³y utworzone, ale jeszcze nie zosta³y zastosowane w bazie danych.

app.Run();

