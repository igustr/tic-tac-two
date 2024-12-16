using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class NewGame : PageModel
{
    [BindProperty] public string GameMode { get; set; } = default!;  // GameMode is selected from radio buttons
    [BindProperty] public string GameType { get; set; } = default!;  // gameType is set from the submit buttons

    public IActionResult OnPost()
    {
        Console.WriteLine("NewGame: " + GameType);
        // Ensure that both GameMode and GameType have been provided before continuing.
        if (string.IsNullOrEmpty(GameMode) || string.IsNullOrEmpty(GameType))
        {
            return Page(); // Stay on the page if one of them is not selected
        }

        Console.WriteLine($"GameMode: {GameMode}, GameType: {GameType}");  // Log selected values

        // Check which button was clicked (gameType = "default" or "custom")
        if (GameType == "default")
        {
            // Redirect to PlayGameWeb with selected GameMode as a parameter
            return RedirectToPage("./PlayGameWeb", new { gameType = "new", gameMode = GameMode });
        }

        if (GameType == "custom")
        {
            // Redirect to custom page for the custom game mode
            return RedirectToPage("./Custom");
        }

        return Page(); // Default return if no valid selection
    }
}