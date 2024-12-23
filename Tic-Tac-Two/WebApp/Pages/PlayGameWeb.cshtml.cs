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

    [BindProperty(SupportsGet = true)] public string GameName { get; set; } = default!;
    [BindProperty(SupportsGet = true)] public string ConfigName { get; set; } = default!;
    public string CurrentAction { get; set; }
    public (int X, int Y)? SelectedPiece { get; set; } = null;
    [BindProperty(SupportsGet = true)] public int GameId { get; set; }
    [BindProperty(SupportsGet = true)] public EGamePiece NextMoveBy { get; set; } = default!;

    public TicTacTwoBrain TicTacTwoBrain { get; set; } = default!;

    public void OnGet(int? x, int? y, string? gameType)
    {
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
        }
        
        // Make a move if coordinates are provided
        if (x != null && y != null)
        {
            //Console.WriteLine("coordinates are provided");
            //Console.WriteLine($"x: {x}, y: {y}");
            TicTacTwoBrain.MakeAMoveCheck(x.Value, y.Value);
            TicTacTwoBrain.MakeAMove(x.Value, y.Value);
            GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "gameName");
            //Console.WriteLine("saved game id: " + GameId);
        }

        TicTacTwoBrain.GridPlacement();
        
        
        
        //CurrentAction = "SelectAction";
        
        //Console.WriteLine("MovePieceAfterNMoves " + TicTacTwoBrain.MovePieceAfterNMoves);
        /*
        Console.WriteLine("grid X coords: " + string.Join(", ", TicTacTwoBrain.GridXCoordinates));
        Console.WriteLine("grid Y coords: " + string.Join(", ", TicTacTwoBrain.GridYCoordinates));
        */

    }
    

    public IActionResult OnPost(int? x, int? y, string action)
    {
        var gameState = _gameRepository.LoadGame(GameId);
        TicTacTwoBrain = new TicTacTwoBrain(gameState);
        var actionList = new List<string> { "U", "D", "L", "R", "UL", "UR", "DL", "DR" };
        

        CurrentAction = action;
        if (actionList.Contains(CurrentAction))
        {
            TicTacTwoBrain.MoveGrid(CurrentAction);
            CurrentAction = "SelectAction";
            GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "gameName");
        }
        if (CurrentAction == "SelectPiece" && x != null && y!= null)
        {
            // Set the selected piece
            if (TicTacTwoBrain.GameBoard[x.Value][y.Value] != EGamePiece.Empty)
            {
                SelectedPiece = (x.Value, y.Value);
                CurrentAction = "MovePiece";
            }
        }
        else if (CurrentAction == "MovePiece" && x != null && y!= null && SelectedPiece != null)
        {
            // Validate move
            var (selectedX, selectedY) = SelectedPiece.Value;
            if (TicTacTwoBrain.IsMoveValidWeb(selectedX, selectedY, x.Value, y.Value))
            {
                // Move the piece and save game
                TicTacTwoBrain.MovePieceWeb(selectedX, selectedY, x.Value, y.Value);
                GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "gameName");
            }

            // Reset state
            SelectedPiece = null;
            CurrentAction = "SelectAction";
        }

        return Page();
    }
}
