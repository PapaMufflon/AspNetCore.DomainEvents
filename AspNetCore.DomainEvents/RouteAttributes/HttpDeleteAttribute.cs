using System;

namespace AspNetCore.DomainEvents.RouteAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HttpDeleteAttribute : HttpRouteAttribute
    {
        public HttpDeleteAttribute(string route) : base(route)
        {
        }
    }
}