namespace DAL;

public static class FileHandler
{
    
    public static string BasePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        "Tic-Tac-Two"
    ) + Path.DirectorySeparatorChar;

    public static string ConfigExtension = ".config.json";
    
    public static string GameExtension = ".game.json";
    
}