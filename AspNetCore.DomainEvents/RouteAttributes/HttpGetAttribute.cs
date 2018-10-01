using System;

namespace AspNetCore.DomainEvents.RouteAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HttpGetAttribute : HttpRouteAttribute
    {
        public HttpGetAttribute(string route) : base(route)
        {
        }
    }
}