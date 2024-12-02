// See https://aka.ms/new-console-template for more information

using DAL;
using Microsoft.EntityFrameworkCore;


var connectionString = $"Data Source={FileHandler.BasePath}app.db";


var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite(connectionString)
    .EnableDetailedErrors()
    .EnableSensitiveDataLogging()
    .Options;

Console.WriteLine("here1");

using var ctx = new AppDbContext(contextOptions);

Console.WriteLine("here2");

ctx.Database.Migrate();

Console.WriteLine("here3");

Console.WriteLine($"Games in db {ctx.Games.Count()}");
Console.WriteLine($"Configs in db {ctx.Configurations.Count()}");

foreach (var conf in ctx.Configurations
             .Include(c => c.Games)
             .Where(c=> c.Id == 2))
{
    Console.WriteLine(conf);
}

//Menus.MainMenu.Run();
