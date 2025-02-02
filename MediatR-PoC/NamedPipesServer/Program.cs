using System.IO.Pipes;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Named Pipe Server started.");

        while (true) 
        {
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

                            Console.WriteLine($"Client: {clientMessage}");

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