using System.IO.Pipes;

namespace NamedPipeClientTests
{
    public class NamePipeTests
    {
        NamedPipeClientStream client;
        [SetUp]
        public async Task Setup()
        {
            client = new NamedPipeClientStream(".", "MediatRPipe", PipeDirection.InOut);
            await client.ConnectAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            await client.DisposeAsync();
        }

        [TestCase("hello","HELLO")]
        public async Task CommsWithServerTest(string clientMessage, string expectedServerResponse)
        {
            using (var reader = new StreamReader(client))
            using (var writer = new StreamWriter(client))
            { 
                await writer.WriteLineAsync(clientMessage);
                await writer.FlushAsync();

                var serverResponse = await reader.ReadLineAsync();
                Console.WriteLine($"Server response -> {serverResponse}");
                Assert.That(serverResponse, Is.EqualTo(expectedServerResponse));

                await writer.WriteLineAsync("exit"); //signal exit
                await writer.FlushAsync();

            }
        }
    }
}