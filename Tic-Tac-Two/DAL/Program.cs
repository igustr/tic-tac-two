// See https://aka.ms/new-console-template for more information

using DAL;
using Microsoft.EntityFrameworkCore;


var connectionString = "Data Source=app.db";


Console.WriteLine("Hello, World!");


var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite(connectionString)
    .EnableDetailedErrors()
    .EnableSensitiveDataLogging()
    .Options;

using var ctx = new AppDbContext(contextOptions);

var saveGameCount = ctx.SaveGames.Count();

Console.WriteLine($"Ganes in db {saveGameCount}");
    