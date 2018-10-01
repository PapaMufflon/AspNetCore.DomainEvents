using AspNetCore.DomainEvents;
using AspNetCore.DomainEvents.RouteAttributes;

namespace TestProject.DomainEvents.Values
{
    [HttpPut("")]
    public class UpdateValue
    {
        public int Id { get; }
        public string NewValue { get; }

        public UpdateValue(int id, string newValue)
        {
            Id = id;
            NewValue = newValue;
        }
    }
}