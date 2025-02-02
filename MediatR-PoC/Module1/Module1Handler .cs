using MediatR;
using System.Threading.Tasks;
using System.Threading;
using MediatRModel;

namespace Module1
{
    public class Module1Handler : INotificationHandler<TestNotification>
    {
        public Task Handle(TestNotification notification, CancellationToken cancellationToken)
        {
            notification.Responses.Add($"Module1:{notification.Message}");
            return Task.CompletedTask;
        }
    }

}
