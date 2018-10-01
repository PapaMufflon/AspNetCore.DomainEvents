using System;

namespace AspNetCore.DomainEvents.RouteAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HttpPostAttribute : HttpRouteAttribute
    {
        public HttpPostAttribute(string route) : base(route)
        {
        }
    }
}