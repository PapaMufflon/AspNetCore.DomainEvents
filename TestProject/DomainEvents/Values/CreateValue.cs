using AspNetCore.DomainEvents;
using AspNetCore.DomainEvents.RouteAttributes;

namespace TestProject.DomainEvents.Values
{
    [HttpPost("")]
    public class CreateValue
    {
        public string Value { get; set; }
    }
}