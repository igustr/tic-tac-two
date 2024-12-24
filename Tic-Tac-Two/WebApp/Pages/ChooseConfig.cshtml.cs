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
        
        // Retrieve configurations with IDs and Names
        var selectListData = _configRepository.GetConfigurations()
            .Select(config => new { id = config.Id, value = config.Name })
            .ToList();

        // Create the SelectList using ID for "value" and Name for display
        ConfigSelectList = new SelectList(selectListData, "id", "value");
        return Page();
    }

    public IActionResult OnPost()
    { 
        if (ModelState.IsValid && ConfigName != null)
        {
            // ConfigName will hold the selected configuration ID
            int selectedConfigId = int.Parse(ConfigName);

            return RedirectToPage("./PlayGameWeb", new { configId = selectedConfigId, gameType = "loadConfig" });
        }

        // Reload SelectList in case of errors
        var selectListData = _configRepository.GetConfigurations()
            .Select(config => new { id = config.Id, value = config.Name })
            .ToList();
        ConfigSelectList = new SelectList(selectListData, "id", "value");

        return Page();
    }
}