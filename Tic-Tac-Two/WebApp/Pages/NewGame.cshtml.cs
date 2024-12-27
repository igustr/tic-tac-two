using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class NewGame : PageModel
{
    [BindProperty] public string GameMode { get; set; } = default!;  
    [BindProperty] public string GameType { get; set; } = default!;  

    public IActionResult OnPost()
    {

        if (string.IsNullOrEmpty(GameMode) || string.IsNullOrEmpty(GameType))
        {
            return Page();
        }

        if (GameType == "default" && GameMode == "AIvsAI")
        {
            return RedirectToPage("./PlayGameWebAi", new { gameType = "new"});   
        }
        
        if (GameType == "default")
        {
            return RedirectToPage("./PlayGameWeb", new { gameType = "new", gameMode = GameMode });
        }

        if (GameType == "custom")
        {
            return RedirectToPage("./Custom");
        }

        return Page(); 
    }
}