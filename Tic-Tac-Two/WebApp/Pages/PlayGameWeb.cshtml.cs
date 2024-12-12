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
    [BindProperty(SupportsGet = true)] public EGamePiece NextMoveBy { get; set; } = default!;

    public TicTacTwoBrain TicTacTwoBrain { get; set; } = default!;

    public void OnGet(int? x, int? y, string gameType)
    {
        // Load the game state based on the game type
        if (gameType == "load")
        {
            var gameState = _gameRepository.GetSavedGameByName(GameName);
            TicTacTwoBrain = new TicTacTwoBrain(gameState);
        }
        else if (gameType == "loadConfig")
        {
            var chosenConfig = _configRepository.GetConfigurationByName(ConfigName);
            TicTacTwoBrain = new TicTacTwoBrain(chosenConfig);
        }
        else
        {
            TicTacTwoBrain = new TicTacTwoBrain(new GameConfiguration());
        }

        // Make a move if coordinates are provided
        if (x != null && y != null)
        {
            TicTacTwoBrain.MakeAMove(x.Value, y.Value);
        }
    }

    /*
    public IActionResult OnPostMakeMove(int x, int y)
    {
        // Load the current game state from the repository or session
        var gameState = _gameRepository.GetSavedGameByName(GameName) ??
                        TicTacTwoBrain.GetCurrentState(); // Default to in-memory state if not saved

        // Reinitialize the game brain with the current state
        TicTacTwoBrain = new TicTacTwoBrain(gameState);

        // Make the move
        TicTacTwoBrain.MakeAMove(x, y);

        // Save the updated state back to the repository
       // _gameRepository.SaveGame(GameName, TicTacTwoBrain.GetCurrentState());

        // Redirect to refresh the page with the updated game state
        return RedirectToPage("./PlayGameWeb", new { gameName = GameName, gameType = "load" });
    }
    */
}
