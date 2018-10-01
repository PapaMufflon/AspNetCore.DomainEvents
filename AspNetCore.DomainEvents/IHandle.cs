using System.Threading.Tasks;

namespace AspNetCore.DomainEvents
{
    public interface IHandle<T>
    {
        Task Run(T parameters);
    }
}