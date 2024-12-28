using DAL;
using GameBrain;
using ConsoleUI;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class PlayGameWebAi : PageModel
{
    private readonly IConfigRepository _configRepository;
    private readonly IGameRepository _gameRepository;

    public PlayGameWebAi(IConfigRepository configRepository, IGameRepository gameRepository)
    {
        _configRepository = configRepository;
        _gameRepository = gameRepository;
    }

    [BindProperty(SupportsGet = true)] public string? Error { get; set; }
    [BindProperty(SupportsGet = true)] public int ConfigId { get; set; } = default!;
    [BindProperty(SupportsGet = true)] public int GameId { get; set; }
    public TicTacTwoBrain TicTacTwoBrain { get; set; } = default!;

    public IActionResult OnGet(string? gameType)
    {
        
        OnGetLoadGame(gameType);
        
        TicTacTwoBrain.GridPlacement();

        if (TicTacTwoBrain.CheckWin())
        {
            return RedirectToPage("./EndPage", new {piece = TicTacTwoBrain.NextMoveBy, gameId = GameId});
        }

        return Page();
    }

    public IActionResult OnPost()
    {
        Console.WriteLine("GameId: " + GameId);
        var gameState = _gameRepository.LoadGame(GameId);
        TicTacTwoBrain = new TicTacTwoBrain(gameState);
        
        TicTacTwoBrain.AiMakeAMove();
        GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "aiGame");
        if (TicTacTwoBrain.CheckWin()) 
        { 
            return RedirectToPage("./EndPage", new {piece = TicTacTwoBrain.NextMoveBy, gameId = GameId});
        }
        
        return Page();
    }

    private void OnGetLoadGame(string? gameType)
    {
        if (gameType == "loadConfig")
        {
            var chosenConfig = _configRepository.GetConfigById(ConfigId);
            TicTacTwoBrain = new TicTacTwoBrain(chosenConfig);
            GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, chosenConfig.Name);
        } 
        else if (gameType == "new")
        {
            TicTacTwoBrain = new TicTacTwoBrain(new GameConfiguration());
            GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "aiGame");
        }
        else
        {
            var gameState = _gameRepository.LoadGame(GameId);
            TicTacTwoBrain = new TicTacTwoBrain(gameState);
        }
    }
}
