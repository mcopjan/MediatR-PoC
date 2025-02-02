using MediatR;
using MediatRModel;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Module2
{
    public class Module2Handler : INotificationHandler<TestNotification>
    {
        public Task Handle(TestNotification notification, CancellationToken cancellationToken)
        {
            notification.Responses.Add($"Module2:{notification.Message}");
            return Task.CompletedTask;
        }
    }
}
