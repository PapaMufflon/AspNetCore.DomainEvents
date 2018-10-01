using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.DomainEvents
{
    internal struct RouteWithVerb
    {
        public Verb Verb { get; }
        public string Route { get; }

        public RouteWithVerb(Verb verb, string route)
        {
            Verb = verb;
            Route = route;
        }

        public override string ToString()
        {
            return $"{Verb} {Route}";
        }

        public bool Fits(string otherRoute)
        {
            var segments = Route.ToLowerInvariant().Split('/');
            var otherSegments = otherRoute.ToLowerInvariant().Split('/');

            if (segments.Length != otherSegments.Length)
            {
                return false;
            }
            
            for (var index = 0; index < segments.Length; index++)
            {
                var segment = segments[index];

                if (otherSegments.Length < index + 1)
                {
                    return false;
                }

                var otherSegment = otherSegments[index];

                if (!segment.StartsWith("{") && !segment.Equals(otherSegment))
                {
                    return false;
                }
            }

            return true;
        }

        public Dictionary<string, string> Fit(string otherRoute)
        {
            var result = new Dictionary<string, string>();
            var segments = Route.ToLowerInvariant().Split('/');
            var otherSegments = otherRoute.ToLowerInvariant().Split('/');

            for (var index = 0; index < segments.Length; index++)
            {
                var segment = segments[index];
                var otherSegment = otherSegments[index];

                if (segment.StartsWith("{"))
                {
                    result.Add(segment.Substring(1, segment.Length - 2), otherSegment);
                }
            }

            return result;
        }
    }
}