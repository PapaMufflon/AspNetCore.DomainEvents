using System.Threading.Tasks;
using AspNetCore.DomainEvents;
using TestProject.DomainEvents.Values;

namespace TestProject.Handlers.Values
{
    public class GetValueHandler : IHandle<GetValue>
    {
        private readonly IResponse _response;

        public GetValueHandler(IResponse response)
        {
            _response = response;
        }
        
        public Task Run(GetValue parameters)
        {
            _response.Set(parameters.Id.ToString());
            return Task.CompletedTask;
        }
    }
    
    public class SaveImage : IHandle<CreateValue>
    {
        private readonly IMediator _mediator;

        public SaveImage(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public Task Run(CreateValue parameters)
        {
            // save
            // publish saved
            _mediator.Publish(new ImageSaved{Id = 42});
            
            return Task.CompletedTask;
        }
    }

    public class ImageSaved
    {
        public int Id { get; set; }
    }

    public class SaveMemento : IHandle<CreateValue>
    {
        private readonly IMediator _mediator;
        private readonly IResponse _response;

        public SaveMemento(IMediator mediator, IResponse response)
        {
            _mediator = mediator;
            _response = response;
        }
        
        public async Task Run(CreateValue parameters)
        {
            var savedImageEvent = await _mediator.WaitFor<ImageSaved>();
            
            // persist memento

            _response.Set(savedImageEvent.Id);
        }
    }
}