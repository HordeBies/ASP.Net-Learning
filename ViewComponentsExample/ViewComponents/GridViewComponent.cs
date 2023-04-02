using Microsoft.AspNetCore.Mvc;
using ViewComponentsExample.Models;

namespace ViewComponentsExample.ViewComponents
{
    public class GridViewComponent : ViewComponent
    {
        async public Task<IViewComponentResult> InvokeAsync()
        {
            PersonGridModel model = new PersonGridModel()
            {
                GridTitle = "Persons List",
                Persons = new()
                {
                    new Person() {PersonName = "John", JobTitle = "Manager"},
                    new Person() {PersonName = "Bies", JobTitle = "Asst. Manager"},
                    new Person() {PersonName = "Lourante", JobTitle = "Kapıcı"},
                }
            };
            ViewData["Grid"] = model;
            return View(); //Views/Shared/Components/Grid/Default.cshtml
        }
    }
}
