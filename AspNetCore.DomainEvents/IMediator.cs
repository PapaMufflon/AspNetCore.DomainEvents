using System.Threading.Tasks;

namespace AspNetCore.DomainEvents
{
    public interface IMediator
    {
        void Publish<T>(T domainEvent);
        Task<T> WaitFor<T>();
    }
}