using MediatR;
using MediatRModel;

namespace NamedPipesServer
{
    public class CommandHandler : IRequestHandler<MediatRCommand, MediatRResponse>
    {
        public async Task<MediatRResponse> Handle(MediatRCommand request, CancellationToken cancellationToken)
        {
            // Simulate some processing
            await Task.Delay(500);

            return new MediatRResponse { Result = $"Processed by MediatR handler: {request.Data}" };
        }
    }
   
}
