using Microsoft.AspNetCore.Mvc;

namespace ViewComponentsExample.ViewComponents
{
    public class GridViewComponent : ViewComponent
    {
        async public Task<IViewComponentResult> InvokeAsync()
        {
            return View(); //Views/Shared/Components/Grid/Default.cshtml
        }
    }
}
