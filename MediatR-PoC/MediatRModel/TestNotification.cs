using MediatR;
using System.Collections.Generic;

namespace MediatRModel
{
    public class TestNotification : INotification
    {
        public string Message { get; set; }
        public List<string> Responses { get; } = new List<string>(); // Shared response list
    }
}
