using System;
using System.Linq;
using System.Reflection;
using AspNetCore.DomainEvents.RouteAttributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.DomainEvents
{
    public static class AspNetCoreExtensions
    {
        private static readonly DomainEventManager DomainEventManager = new DomainEventManager();

        public static void UseDomainEvents(this IApplicationBuilder app)
        {
            DomainEventManager.Initialize(app.ApplicationServices);
            
            app.Run(DomainEventManager.Handler);
        }

        public static void AddDomainEvents(this IServiceCollection services, string domainEventsRootNamespace, params Assembly[] assemblies)
        {
            DomainEventManager.RegisterRoutes(domainEventsRootNamespace, assemblies);
            
            services.AddScoped<IMediator, ScopedMediator>();
            services.AddScoped<IResponse, Response>();
            
            foreach(var type in assemblies.SelectMany(x => x.GetTypes()))
            {
                TryAddDomainEvent(services, type);
                TryAddHandler(services, type);
            }
        }

        private static void TryAddDomainEvent(IServiceCollection services, Type type)
        {
            var routeAttribute = type
                .GetCustomAttributes(typeof(HttpRouteAttribute), false)
                .SingleOrDefault() as HttpRouteAttribute;

            if (routeAttribute == null) return;

            services.AddScoped(type);
        }
        
        private static void TryAddHandler(IServiceCollection services, Type type)
        {
            foreach (var handler in type.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IHandle<>)))
            {
                services.AddScoped(type);

                var domainEvent = handler.GenericTypeArguments[0];

                DomainEventManager.RegisterHandler(domainEvent, type);
            }
        }
    }
}