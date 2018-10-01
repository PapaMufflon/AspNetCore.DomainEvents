using AspNetCore.DomainEvents;
using AspNetCore.DomainEvents.RouteAttributes;

namespace TestProject.DomainEvents.Values
{
    [HttpDelete("")]
    public class DeleteValue
    {
        public int Id { get; }

        public DeleteValue(int id)
        {
            Id = id;
        }
    }
}