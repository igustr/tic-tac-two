using GameBrain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp;

public class PlayGameWeb : PageModel
{
    public TicTacTwoBrain TicTacTwoBrain { get; set; } = default!;
    
    public void OnGet()
    {
        GameConfiguration config = new GameConfiguration();
        TicTacTwoBrain = new TicTacTwoBrain(config);
        TicTacTwoBrain.MakeAMove(2, 2);
        TicTacTwoBrain.MakeAMove(1, 1);
    }
}