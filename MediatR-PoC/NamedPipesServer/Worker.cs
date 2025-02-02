using MediatR;
using MediatRModel;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NamedPipesServer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMediator _mediator;
        private const string PipeName = "MediatR_Pipe";

        public Worker(ILogger<Worker> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Named Pipe Server started.");

                Console.WriteLine("Waiting for client to connect...");

                using (var server = new NamedPipeServerStream("MediatRPipe", PipeDirection.InOut))
                {
                    await server.WaitForConnectionAsync();
                    Console.WriteLine("Client connected");

                    try
                    {
                        using (var reader = new StreamReader(server))
                        using (var writer = new StreamWriter(server, leaveOpen: true))
                        {
                            while (true)
                            {
                                string clientMessage = await reader.ReadLineAsync();

                                if (clientMessage == null)
                                {
                                    Console.WriteLine("Client disconnected");
                                    break;
                                }

                                if (clientMessage.Equals("exit", StringComparison.OrdinalIgnoreCase))
                                {
                                    Console.WriteLine("Client requested exit");
                                    break;
                                }

                                var mediatRResponse = _mediator.Send(new MediatRCommand() { Data = clientMessage });
                                Console.WriteLine($"Client: {clientMessage}");
                                Console.WriteLine($"Mediatr response: {mediatRResponse.Result.Result}");

                                //example of processing a message and returing back to the client
                                string serverResponse = clientMessage.ToUpper();
                                await writer.WriteLineAsync(serverResponse);
                                await writer.FlushAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occured during communication: {ex.Message}");
                    }
                }

            }
        }
    }
}
