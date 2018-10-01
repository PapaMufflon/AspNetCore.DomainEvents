using System;
using AspNetCore.DomainEvents.RouteAttributes;

namespace AspNetCore.DomainEvents
{
    internal enum Verb
    {
        Get,
        Post,
        Put,
        Delete
    }

    internal static class VerbExtensions
    {
        public static Verb VerbFromAttributeType(this Type type)
        {
            if (type == typeof(HttpGetAttribute))
            {
                return Verb.Get;
            }
                    
            if (type == typeof(HttpDeleteAttribute))
            {
                return Verb.Delete;
            }
                    
            if (type == typeof(HttpPutAttribute))
            {
                return Verb.Put;
            }
                    
            if (type == typeof(HttpPostAttribute))
            {
                return Verb.Post;
            }
            
            throw new ArgumentException($"Type {type} is not a valid HttpRouteAttribute-Type.");
        }
        
        public static Verb VerbFromRequestMethod(this string requestMethod)
        {
            if (requestMethod.ToLowerInvariant().Equals("get"))
            {
                return Verb.Get;
            }
                    
            if (requestMethod.ToLowerInvariant().Equals("delete"))
            {
                return Verb.Delete;
            }
                    
            if (requestMethod.ToLowerInvariant().Equals("put"))
            {
                return Verb.Put;
            }
                    
            if (requestMethod.ToLowerInvariant().Equals("post"))
            {
                return Verb.Post;
            }
            
            throw new ArgumentException($"Request method {requestMethod} is not available.");
        }
    }
}