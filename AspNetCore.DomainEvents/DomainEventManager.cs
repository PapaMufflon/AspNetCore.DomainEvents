using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspNetCore.DomainEvents.RouteAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace AspNetCore.DomainEvents
{
    internal class DomainEventManager
    {
        private readonly Mediator _mediator = new Mediator();
        
        private Dictionary<RouteWithVerb, Type> _routes;
        private IServiceProvider _serviceProvider;

        public void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public void RegisterRoutes(string domainEventsRootNamespace, Assembly[] assemblies)
        {
            _routes = new Dictionary<RouteWithVerb, Type>();

            foreach(var type in assemblies.SelectMany(x => x.GetTypes()))
            {
                var routeAttribute = type.GetCustomAttributes(typeof(HttpRouteAttribute), false).SingleOrDefault() as HttpRouteAttribute;

                if (routeAttribute == null) continue;

                var route = '/' + type.Namespace.Substring(domainEventsRootNamespace.Length + 1);

                if (!string.IsNullOrEmpty(routeAttribute.Route))
                {
                    route += '/' + routeAttribute.Route; 
                }
                
                _routes.Add(new RouteWithVerb(routeAttribute.GetType().VerbFromAttributeType(), route), type);
            }
        }
        
        public async Task Handler(HttpContext context)
        {
            var path = context.Request.Path;
            var verb = context.Request.Method.VerbFromRequestMethod();

            var routeWithVerb = _routes.Keys.FirstOrDefault(x => x.Verb == verb && x.Fits(path));

            if (routeWithVerb.Equals(default(RouteWithVerb)))
            {
                return;
            }

            var parametersAsString = routeWithVerb.Fit(path);
            
            var rawJson = new StreamReader(context.Request.Body).ReadToEnd().ToLowerInvariant();
            var json = JObject.Parse(rawJson);
            
            var action = _routes[routeWithVerb];
            var properties = action.GetProperties();

            var scopedServiceProvider = _serviceProvider.CreateScope().ServiceProvider;
            var domainEvent = scopedServiceProvider.GetService(action);
            var response = scopedServiceProvider.GetService<IResponse>();

            parametersAsString
                .Keys
                .Where(x => properties.Any(p => p.Name.ToLowerInvariant().Equals(x)))
                .Select(x => action.GetProperty(x))
                .ToList()
                .ForEach(x => x.SetValue(domainEvent, Convert.ChangeType(parametersAsString[x.Name], x.PropertyType)));
            
            properties
                .Where(x => json.Properties().Any(p => p.Name.ToLowerInvariant().Equals(x.Name.ToLowerInvariant())))
                .ToList()
                .ForEach(x => x.SetValue(domainEvent, Convert.ChangeType(json[x.Name.ToLowerInvariant()], x.PropertyType)));
                
            _mediator.Publish(domainEvent, scopedServiceProvider);

            var payload = await response.Completed();

            if (payload != null)
            {
                await context.Response.WriteAsync(payload.ToString());
            }
        }

        public void RegisterHandler(Type domainEvent, Type type)
        {
            _mediator.Register(domainEvent, type);
        }
    }
}