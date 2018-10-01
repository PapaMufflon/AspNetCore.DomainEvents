using AspNetCore.DomainEvents;
using AspNetCore.DomainEvents.RouteAttributes;

namespace TestProject.DomainEvents.Values
{
    [HttpGet("{id}")]
    public class GetValue
    {
        public int Id { get; set; }
    }
}