using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    [BindProperty(SupportsGet = true)]
    public string? Error { get; set; }
    

    public void OnGet()
    {
    }
    
    public IActionResult OnPostNewGame()
    {
        return RedirectToPage("./PlayGameWeb");
    }

    public IActionResult OnPostLoadGame()
    {
        return RedirectToPage("./Load");
    }

    public IActionResult OnPostConfigurations()
    {
        return RedirectToPage("./ChooseConfig");
    }

    public IActionResult OnPostConnectToGame()
    {
        return RedirectToPage("./Home");
    }
   
}