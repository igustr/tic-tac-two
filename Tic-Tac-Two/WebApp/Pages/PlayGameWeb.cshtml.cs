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
    [BindProperty(SupportsGet = true)] public int GameId { get; set; } = default!;
    [BindProperty(SupportsGet = true)] public EGamePiece NextMoveBy { get; set; } = default!;

    public TicTacTwoBrain TicTacTwoBrain { get; set; } = default!;

    public void OnGet(int? x, int? y, string? gameType)
    {
        // Load the game state based on the game type
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
            Console.WriteLine("coordinates are provided");
            Console.WriteLine($"x: {x}, y: {y}");
            TicTacTwoBrain.MakeAMoveCheck(x.Value, y.Value);
            TicTacTwoBrain.MakeAMove(x.Value, y.Value);
            GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "gameName");
            Console.WriteLine("saved game id: " + GameId);
        }

        TicTacTwoBrain.GridPlacement();
        
        /*
        Console.WriteLine("grid X coords: " + string.Join(", ", TicTacTwoBrain.GridXCoordinates));
        Console.WriteLine("grid Y coords: " + string.Join(", ", TicTacTwoBrain.GridYCoordinates));
        */

    }


    /*
    public IActionResult OnPost(int x, int y, GameState gameState)
    {
        // Load the current game state from the repository or session

        // Reinitialize the game brain with the current state
        TicTacTwoBrain = new TicTacTwoBrain(gameState);

        // Make the move
        TicTacTwoBrain.MakeAMove(x, y);

        return RedirectToPage("./PlayGameWeb", new { currentGameState = gameState, gameType = "continue" });
    }
    */
}
