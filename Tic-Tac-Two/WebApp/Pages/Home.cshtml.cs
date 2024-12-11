
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace WebApp.Pages;

public class Home : PageModel
{
    private readonly ILogger<Home> _logger;

    public Home(ILogger<Home> logger)
    {
        _logger = logger;
    }

    [BindProperty(SupportsGet = true)]
    public string? Error { get; set; }

    [BindProperty]
    public string? Password { get; set; }

    public void OnGet()
    {
    }
    
    public IActionResult OnPost()
    {
        Password = Password?.Trim();
        
        if (!string.IsNullOrWhiteSpace(Password)) //TODO: and check if game with this password exists 
        {
            return RedirectToPage("./PlayGameWeb", new { password = Password });
        }
        
        Error = "Please enter a password.";
        
        return Page();
    }
}