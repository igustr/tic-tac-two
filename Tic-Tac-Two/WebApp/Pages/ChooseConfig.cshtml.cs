using DAL;
using Domain;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages;

public class ChooseConfig : PageModel
{
    private readonly IConfigRepository _configRepository;

    public ChooseConfig(IConfigRepository configRepository)
    {
        _configRepository = configRepository;
    }
    [BindProperty]
    public string? ConfigName { get; set; } 
    public SelectList ConfigSelectList { get; set; } = default!;

    
    public IActionResult OnGet()
    {
        var selectListData = _configRepository.GetConfigurationNames()
            .Select(name => new {id = name, value = name})
            .ToList();
        
        ConfigSelectList = new SelectList(selectListData, "id", "value");
        return Page();
    }

    public IActionResult OnPost()
    { 
        if (ModelState.IsValid)
        {
            // Reload the GamesSelectList in case of validation errors
            var selectListData = _configRepository.GetConfigurationNames()
                .Select(name => new SelectListItem { Value = name, Text = name })
                .ToList();

            ConfigSelectList = new SelectList(selectListData, "id", "value");
            return RedirectToPage("./PlayGameWeb", new { configName = ConfigName, gameType = "loadConfig" });
        }
        
        return Page();
    }
}