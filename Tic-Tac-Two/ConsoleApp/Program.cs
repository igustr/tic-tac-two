// See https://aka.ms/new-console-template for more information

using ConsoleApp;
using DAL;
using Microsoft.EntityFrameworkCore;


//Menus.MainMenu.Run();

/*
var connectionString = $"Data Source={FileHandler.BasePath}app.db";

var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite(connectionString)
    .EnableDetailedErrors()
    .EnableSensitiveDataLogging()
    .Options;

using var ctx = new AppDbContext(contextOptions);

ctx.Database.Migrate();

Console.WriteLine($"Games in db {ctx.Games.Count()}");
Console.WriteLine($"Configs in db {ctx.Configurations.Count()}");

foreach (var conf in ctx.Configurations
             .Include(c => c.Games)
             .Where(c=> c.Id == 2))
{
    Console.WriteLine(conf);
}
*/

var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

var connectionString = $"Data Source={FileHandler.BasePath}app.db";
optionsBuilder.UseSqlite(connectionString);

using var db = new AppDbContext(optionsBuilder.Options);
  

var configRepository = new ConfigRepositoryDB(db);
var gameRepository = new GameRepositoryDB(db);


//var configRepository = new ConfigRepositoryJson();
//var gameRepository = new GameRepositoryJson();

Console.WriteLine($"Games in db {db.Games.Count()}");
Console.WriteLine($"Configs in db {db.Configurations.Count()}");

Menus.Init(configRepository, gameRepository);
Menus.MainMenu.Run();
