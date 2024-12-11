using DAL;
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

    public SelectList ConfigSelectList { get; set; } = default!;

    [BindProperty]
    public int ConfigId { get; set; }
    
    public IActionResult OnGet()
    {

        var selectListData = _configRepository.GetConfigurationNames()
            .Select(name => new {id = name, value = name})
            .ToList();
        
        ConfigSelectList = new SelectList(selectListData, "id", "value");
        
        return Page();
    }

}