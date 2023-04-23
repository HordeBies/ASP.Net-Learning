using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SkipFilter : Attribute, IFilterMetadata
    {
    }
}
