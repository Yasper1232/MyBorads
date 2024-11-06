using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using MyBorads.Entities;
using System.Diagnostics.Metrics;
using System.IO;
using System.Net;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddDbContext<MyBoardsContext>(
    option => option
    .UseLazyLoadingProxies()
    .UseSqlServer(builder.Configuration.GetConnectionString("MyBoardsConnectionString"))
    
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
        Address = new Address()
        {
            City = "Warszawa",
            Street = "Szeroka"
        }
    };
    var user2 = new User()
    {
        Email = "user2@test.com",
        FullName = "User two",
        Address = new Address()
        {
            City = "Krakow",
            Street = "Dluga"
        }
    };
    dbContext.Users.AddRange(user1, user2);
    dbContext.SaveChanges();
}

//Metoda GetPendingMigrations() jest cz�ci� API migracji Entity Framework Core. S�u�y do uzyskania listy migracji, //kt�re zosta�y utworzone, ale jeszcze nie zosta�y zastosowane w bazie danych.

app.MapGet("data",async (MyBoardsContext db) =>
{

    var withAddress = true;

    var users = db.Users
    
    .First(u => u.Id == Guid.Parse("78CF834E-7724-4995-CBC4-08DA10AB0E61"));

    if (withAddress)
    {
        var result = new { Fullname = users.FullName, Address = $"{users.Address.Street} {users.Address.City}" };
        return result;
    }

    return new {FullName = users.FullName, Address = "-"};


});

app.MapPost("update", async (MyBoardsContext db) =>
{
    Epic epic = await db.Epics.FirstAsync(epic => epic.Id == 1);

    var rejectedState = await db.WorkItemsStates.FirstAsync(a => a.Value == "Rejected");

    epic.State = rejectedState;

   await db.SaveChangesAsync();

    return epic;

});

app.MapPost("create", async (MyBoardsContext db) =>
{
    var address = new Address()
    {   
        Id = Guid.Parse("2069cd89-e101-47e3-92e0-de1ac3d35328"),
        City = "Krak�w",
        Country = "Poland",
        Street = "D�uga"

    };

    var user = new User()
    {
        Email = "user@test.com",
        FullName = "Test User",
        Address = address,
    };

    db.Users.Add(user);
    await db.SaveChangesAsync();
    return user;
});

app.MapDelete("Delete", async(MyBoardsContext db) =>
{
    var user = await db.Users
    .Include(u=>u.Comments)
    .FirstAsync(u => u.Id == Guid.Parse("4EBB526D-2196-41E1-CBDA-08DA10AB0E61"));

    db.Users.Remove(user);


    await db.SaveChangesAsync();

});



app.Run();

