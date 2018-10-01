using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.DomainEvents
{
    internal class ScopedMediator : IMediator
    {
        private readonly Dictionary<Type, EventState> _published = new Dictionary<Type, EventState>();
        
        public void Publish<T>(T domainEvent)
        {
            if (!_published.ContainsKey(typeof(T)))
            {
                _published.Add(typeof(T), new EventState(true));
            }

            _published[typeof(T)].DomainEvent = domainEvent;
            _published[typeof(T)].ManualResetEvent.Set();
        }

        public Task<T> WaitFor<T>()
        {
            if (!_published.ContainsKey(typeof(T)))
            {
                _published.Add(typeof(T), new EventState(false));
            }

            _published[typeof(T)].ManualResetEvent.WaitOne();
            return Task.FromResult((T) _published[typeof(T)].DomainEvent);
        }

        private class EventState
        {
            public ManualResetEvent ManualResetEvent { get; private set; }
            public object DomainEvent { get; set; }

            public EventState(bool set)
            {
                ManualResetEvent = new ManualResetEvent(set);
            }
        }
    }
}