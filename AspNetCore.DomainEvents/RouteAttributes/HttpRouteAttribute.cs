using System;

namespace AspNetCore.DomainEvents.RouteAttributes
{
    public class HttpRouteAttribute : Attribute
    {
        public string Route { get; }

        public HttpRouteAttribute(string route)
        {
            Route = route;
        }
    }
}