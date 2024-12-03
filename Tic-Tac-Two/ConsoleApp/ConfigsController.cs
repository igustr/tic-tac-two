using DAL;
using MenuSystem;
using GameBrain;

namespace ConsoleApp;

public static class ConfigsController
{
    private static IConfigRepository _configRepository = default!;
    
    public static string ChooseSavedConfiguration(IConfigRepository configRepository, IGameRepository gameRepository)
    {
        _configRepository = configRepository;
        
        var configMenuItems = new List<MenuItem>();

        for (var i = 0; i < _configRepository.GetConfigurationNames().Count; i++)
        {
            var returnValue = i.ToString();
            configMenuItems.Add(new MenuItem()
            {
                Title = _configRepository.GetConfigurationNames()[i],
                Shortcut = (i + 1).ToString(),
                MenuItemAction = () => returnValue
            });
        }
        
        var configMenu = new Menu(EMenuLevel.Secondary,
            "TIC-TAC-TOE - choose game config",
            configMenuItems,
            isCustomMenu: true
        );

        var chosenConfig = configMenu.Run();

        GameController.MainLoop(chosenConfig, "new", configRepository, gameRepository);
        return "new";
    }

    public static GameConfiguration CustomGameCheck()
    {
        var customConfig = new GameConfiguration();
        
        // Min board size 5x5
        bool isValidBoardSize = false;
        while (!isValidBoardSize)
        {
            Console.WriteLine("Board size: ");
            var boardSize = int.Parse(Console.ReadLine());

            if (boardSize >= 5)
            {
                customConfig.BoardSizeHeight = boardSize;
                customConfig.BoardSizeWidth = boardSize;
                isValidBoardSize = true;
            }
            else
            {
                Console.WriteLine("Invalid board size! The board size must be at least 5."); 
                
            }
        }
        // Min grid size 3x3, grid smaller than board at least 2.
        bool isValidGridSize = false;
            while (!isValidGridSize)
            {
                Console.WriteLine("Grid Size: ");
                var gridSize = int.Parse(Console.ReadLine());

                if (gridSize >= 3 && gridSize < customConfig.BoardSizeHeight)
                {
                    customConfig.GridSizeHeight = gridSize;
                    customConfig.GridSizeWidth = gridSize;
                    isValidGridSize = true;
                }
                else
                {
                    Console.WriteLine("Invalid grid size! The grid size must be at least 3 and smaller than board size.");
                }
            }

            // Min amount of pieces 3
            bool isValidAmountOfPieces = false;
            while (!isValidAmountOfPieces)
            {
                Console.WriteLine("Amount of Pieces: ");
                customConfig.AmountOfPieces = int.Parse(Console.ReadLine());

                if (customConfig.AmountOfPieces >= 3)
                {
                    customConfig.MovePieceAfterNMoves = customConfig.AmountOfPieces / 2;
                    isValidAmountOfPieces = true;
                }
                else
                {
                    Console.WriteLine("Invalid amount of pieces! The amount must be at least 3.");
                }
            }

            // Must be less than or equal to Amount of Pieces
            bool isValidWinCondition = false;
            while (!isValidWinCondition)
            {
                Console.WriteLine("Amount of Pieces to Win: ");
                customConfig.WinCondition = int.Parse(Console.ReadLine());

                if (customConfig.WinCondition <= customConfig.AmountOfPieces 
                    && customConfig.WinCondition > 0 && customConfig.WinCondition <= customConfig.GridSizeHeight)
                {
                    isValidWinCondition = true;
                }
                else
                {
                    Console.WriteLine(
                        "Invalid input! Pieces to Win should be less than or equal to the Amount of Pieces, " +
                        "greater than 0 and less than or equal to grid Size - " + customConfig.GridSizeHeight);
                }
            }
            return customConfig;
    }

}