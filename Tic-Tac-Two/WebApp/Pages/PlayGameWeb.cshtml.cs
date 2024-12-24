using DAL;
using GameBrain;
using ConsoleUI;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class PlayGameWeb : PageModel
{
    private readonly IConfigRepository _configRepository;
    private readonly IGameRepository _gameRepository;

    public PlayGameWeb(IConfigRepository configRepository, IGameRepository gameRepository)
    {
        _configRepository = configRepository;
        _gameRepository = gameRepository;
    }

    [BindProperty(SupportsGet = true)]
    public string? Error { get; set; }
    [BindProperty(SupportsGet = true)] public string GameName { get; set; } = default!;
    [BindProperty(SupportsGet = true)] public string ConfigName { get; set; } = default!;
    public string CurrentAction { get; set; }
    [BindProperty(SupportsGet = true)] public int SelectedX { get; set; }
    [BindProperty(SupportsGet = true)] public int SelectedY { get; set; } 
    [BindProperty(SupportsGet = true)] public int GameId { get; set; }
    [BindProperty(SupportsGet = true)] public EGamePiece NextMoveBy { get; set; } = default!;

    public TicTacTwoBrain TicTacTwoBrain { get; set; } = default!;

    public IActionResult OnGet(int? x, int? y, string? gameType)
    {
        Console.WriteLine("currentaction get: " + gameType);
        Console.WriteLine("selectPiece<x,y> GET: " + SelectedX + "," + SelectedY);

        // Load game state based on the game type
        if (gameType == "load")
        {
            var gameState = _gameRepository.GetSavedGameByName(GameName);
            TicTacTwoBrain = new TicTacTwoBrain(gameState);
            GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "gameName");
        }
        else if (gameType == "loadConfig")
        {
            var chosenConfig = _configRepository.GetConfigurationByName(ConfigName);
            TicTacTwoBrain = new TicTacTwoBrain(chosenConfig);
            GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "gameName");
        } 
        else if (gameType == "new")
        {
            TicTacTwoBrain = new TicTacTwoBrain(new GameConfiguration());
            GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "gameName");
        }
        else
        {
            Console.WriteLine("game id: " + GameId);
            var gameState = _gameRepository.LoadGame(GameId);
            TicTacTwoBrain = new TicTacTwoBrain(gameState);
            

            if (FinalStageCheck(TicTacTwoBrain) && gameType != "MovePiece")
            {
                Console.WriteLine("redirecting to final stage");
                CurrentAction = "SelectPiece";
                gameType = "SelectPiece";
            }
        }
        if (gameType == "MovePiece" && x != null && y != null)
        {
            Console.WriteLine("here in MovePiece");
            
            TicTacTwoBrain.MovePieceWeb(SelectedX, SelectedY, x.Value, y.Value);
            CurrentAction = "SelectAction";
            GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "gameName");
        }
        else if (x != null && y != null && gameType != "MovePiece" && gameType != "SelectPiece")
        {
            //Console.WriteLine("coordinates are provided");
            //Console.WriteLine($"x: {x}, y: {y}");
            if (!TicTacTwoBrain.MakeAMoveCheck(x.Value, y.Value))
            {
                Error = "You can't place piece outside of grid!";
                CurrentAction = "SelectAction";
                GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "gameName");
                return Page();
            }
            TicTacTwoBrain.MakeAMove(x.Value, y.Value);
            GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "gameName");
            //Console.WriteLine("saved game id: " + GameId);
            // Clear the values after use
        } 

        TicTacTwoBrain.GridPlacement();
        
        
        //CurrentAction = "SelectAction";
        
        //Console.WriteLine("MovePieceAfterNMoves " + TicTacTwoBrain.MovePieceAfterNMoves);
        
        /*
        Console.WriteLine("grid X coords: " + string.Join(", ", TicTacTwoBrain.GridXCoordinates));
        Console.WriteLine("grid Y coords: " + string.Join(", ", TicTacTwoBrain.GridYCoordinates));
        */

        return Page();
    }
    

    public IActionResult OnPost(int? x, int? y, string action)
    {
        var gameState = _gameRepository.LoadGame(GameId);
        TicTacTwoBrain = new TicTacTwoBrain(gameState);
        var actionList = new List<string> { "U", "D", "L", "R", "UL", "UR", "DL", "DR" };
        
        CurrentAction = action;
        
        Console.WriteLine("action: " + CurrentAction);
        
        if (actionList.Contains(CurrentAction))
        {

            TicTacTwoBrain.MoveGrid(CurrentAction);
            if (!TicTacTwoBrain.GridPlacement())
            {
                Console.WriteLine("here in MoveGridCheckWeb");
                Error = "You can't move grid in this direction!";
                CurrentAction = "SelectAction";
                GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "gameName");
                return Page();
            }
            CurrentAction = "SelectAction";
            GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "gameName");
        } 
        else if (CurrentAction == "ChoosePiece" && x != null && y != null)
        {
            Console.WriteLine("here in ChoosePiece");

            SelectedX = x.Value;
            SelectedY = y.Value;

            CurrentAction = "MovePiece";
        } 

        return Page();
    }

    public bool FinalStageCheck(TicTacTwoBrain gameInstance)
    {
        if (gameInstance.NextMoveBy == EGamePiece.X && gameInstance.GetPiecesOnBoardCount().Item1 == gameInstance.AmountOfPieces)
        {
            return true;
        }
            
        if (gameInstance.NextMoveBy == EGamePiece.O && gameInstance.GetPiecesOnBoardCount().Item2 == gameInstance.AmountOfPieces)
        {
            return true;
        }

        return false;
    }
    
}
