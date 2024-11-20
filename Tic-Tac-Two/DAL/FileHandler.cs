namespace DAL;

public static class FileHandler
{
    public static string BasePath = Environment
                                        .GetFolderPath(System.Environment.SpecialFolder.UserProfile)
                                    + Path.DirectorySeparatorChar + "Tic-Tac-Two" + Path.DirectorySeparatorChar;
    
    public static string ConfigExtension = ".config.json";
    public static string GameExtension = ".game.json";

    
}