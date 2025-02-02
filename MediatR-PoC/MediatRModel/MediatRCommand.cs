using MediatR;

namespace MediatRModel
{
    public class MediatRCommand : IRequest<MediatRResponse>
    {
        public string Data { get; set; }
    }
}
