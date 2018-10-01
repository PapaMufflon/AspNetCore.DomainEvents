using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.DomainEvents
{
    internal class Response : IResponse
    {
        private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(false);
        private object _payload;

        public Task<object> Completed()
        {
            _autoResetEvent.WaitOne();
            
            return Task.FromResult(_payload);
        }

        public void Set(object response)
        {
            _payload = response;
            _autoResetEvent.Set();
        }
    }
}