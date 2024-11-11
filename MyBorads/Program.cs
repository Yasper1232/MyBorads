using LinqToDB.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using MyBorads;
using MyBorads.Dto;
using MyBorads.Entities;
using MyBorads.Migrations;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq.Expressions;
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
    //.UseLazyLoadingProxies()
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

DataGenerator.Seed(dbContext);

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

//Metoda GetPendingMigrations() jest czêœci¹ API migracji Entity Framework Core. S³u¿y do uzyskania listy migracji, //które zosta³y utworzone, ale jeszcze nie zosta³y zastosowane w bazie danych.

app.MapGet("pagination", async (MyBoardsContext db) =>
{
    //user input
    var filter = "a";
    string sortBy = "FullName"; //fullname,email,null ?
    bool sortByDescending = false;
    int pageNumber = 1;
    int pageSize = 10;
    //

    var query = db.Users
    .Where(u => filter == null || (u.Email.Contains(filter.ToLower()) || u.FullName.Contains(filter.ToLower())));

    var totalCount = query.Count();

    if(sortBy != null)
    {
        var columnsSelector = new Dictionary<string, Expression<Func<User, object>>>
        {

            {nameof(User.Email),user => user.Email},
            {nameof(User.FullName),user => user.FullName},
        };

        var sortByExpression = columnsSelector[sortBy];

        query = sortByDescending 
        ? query.OrderByDescending(sortByExpression) 
        : query.OrderBy(sortByExpression);



    }

    var result = query.Skip(pageSize * (pageSize - 1))
    .Take(pageSize)
    .ToList();

    var pagedResult = new PagedResult<User>(result,totalCount,pageSize,pageNumber);

    return pagedResult;

});
    


app.MapGet("data",async (MyBoardsContext db) =>
{

    var usercomments = await db.Users
    .Include(u => u.Address)
    .Include(u => u.Comments)
    .Where(u => u.Address.Country == "Albania")
    .SelectMany(u=>u.Comments)
    .Select(c=>c.Message)
    .ToListAsync();


    return usercomments;

});

app.MapPost("update", async (MyBoardsContext db) =>
{
    Epic epic = await db.Epics.FirstAsync(epic => epic.Id == 1);

    var rejectedState = await db.WorkItemsStates.FirstAsync(a => a.Value == "Rejected");

    epic.State = rejectedState;

   await db.SaveChangesAsync();

    return epic;

});

app.MapPut("updateLinq2Db", async (MyBoardsContext db) =>
{

    var comments = db.Comments
    .Where(c => c.CreatedDate > new DateTime(2022, 4, 20));

    await LinqToDB.LinqExtensions.UpdateAsync(comments.ToLinqToDB(), x => new Comment
    {
        Message = "Great comment"
    });
});


app.MapPost("create", async (MyBoardsContext db) =>
{
    var address = new Address()
    {   
        Id = Guid.Parse("2069cd89-e101-47e3-92e0-de1ac3d35328"),
        City = "Kraków",
        Country = "Poland",
        Street = "D³uga"

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

