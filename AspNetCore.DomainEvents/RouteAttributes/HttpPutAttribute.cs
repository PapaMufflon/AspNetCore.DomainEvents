using System;

namespace AspNetCore.DomainEvents.RouteAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HttpPutAttribute : HttpRouteAttribute
    {
        public HttpPutAttribute(string route) : base(route)
        {
        }
    }
}