using DAL;
using GameBrain;
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


    [BindProperty(SupportsGet = true)] public int GameId { get; set; } = default!;

    [BindProperty(SupportsGet = true)] public EGamePiece NextMoveBy { get; set; } = default!;

    public TicTacTwoBrain TicTacToeBrain { get; set; } = default!;

    /*
    public void OnGet(int? x, int? y)
    {
        var dbGame = _gameRepository.LoadGame(GameId);
        TicTacToeBrain = new TicTacTwoBrain(new GameConfiguration()
        {
            Name = "Classical"
        });

        TicTacToeBrain.SetGameStateJson(dbGame.GameState);

        if (x != null && y != null)
        {
            var userGameName = "Game1 (ChangeThat)";
            TicTacToeBrain.MakeAMove(x.Value, y.Value);
            GameId = _gameRepository.SaveGame(TicTacToeBrain.GetGameStateJson(),TicTacToeBrain.GetGameConfigName(), userGameName);
        }

    }
    */
}