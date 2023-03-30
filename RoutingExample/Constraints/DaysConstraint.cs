namespace RoutingExample.Constraints
{
    public class DaysConstraint : IRouteConstraint
    {
        private readonly string[] days = new string[] { "monday", "tuesday", "wednesday", "thursday","friday","saturday","sunday" };

        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return values.ContainsKey(routeKey) && days.Contains(values[routeKey]!.ToString());
        }
    }
}
