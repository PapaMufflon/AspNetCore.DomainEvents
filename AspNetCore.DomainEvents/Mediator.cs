using System;
using System.Collections.Generic;

namespace AspNetCore.DomainEvents
{
    internal class Mediator
    {
        private readonly Dictionary<Type, List<Type>> _domainEventHandlers = new Dictionary<Type, List<Type>>();

        public void Register(Type domainEvent, Type handler)
        {
            if (!_domainEventHandlers.ContainsKey(domainEvent))
            {
                _domainEventHandlers.Add(domainEvent, new List<Type>());
            }
            
            _domainEventHandlers[domainEvent].Add(handler);
        }

        public void Publish(object domainEvent, IServiceProvider serviceProvider)
        {
            var type = domainEvent.GetType();

            foreach (var handlerType in _domainEventHandlers[type])
            {
                var handler = serviceProvider.GetService(handlerType);
                var methodInfo = handler.GetType().GetMethod("Run");

                methodInfo.Invoke(handler, new[] { domainEvent });
            }
        }
    }
}