using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.UI.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SkipFilter : Attribute, IFilterMetadata
    {
    }
}
