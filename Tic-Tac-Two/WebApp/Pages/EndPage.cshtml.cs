using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class EndPage : PageModel
{
    public string? Winner { get; set; }
    
    public void OnGet(string? piece)
    {
        Winner = piece;
    }
}