using System.Threading.Tasks;

namespace AspNetCore.DomainEvents
{
    public interface IResponse
    {
        Task<object> Completed();
        void Set(object response);
    }
}