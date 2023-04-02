using Microsoft.AspNetCore.Mvc;
using ViewComponentsExample.Models;

namespace ViewComponentsExample.ViewComponents
{
    public class GridViewComponent : ViewComponent
    {
        async public Task<IViewComponentResult> InvokeAsync(PersonGridModel model)
        {
            return View(model); //Views/Shared/Components/Grid/Default.cshtml
        }
    }
}
