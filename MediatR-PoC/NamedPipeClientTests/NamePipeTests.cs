using NUnit.Framework.Legacy;
using System.IO.Pipes;
using System.Text.Json;

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

        [TestCase("hello", new string[] { "Module1:hello", "Module2:hello" })]
        public async Task CommsWithServerTest(string clientMessage, string[] expectedServerResponse)
        {
            using (var reader = new StreamReader(client))
            using (var writer = new StreamWriter(client))
            { 
                await writer.WriteLineAsync(clientMessage);
                await writer.FlushAsync();

                var serverJsonResponse = await reader.ReadLineAsync();
                Console.WriteLine($"Server response -> {serverJsonResponse}");
                List<string>? responses = JsonSerializer.Deserialize<List<string>>(serverJsonResponse);
                CollectionAssert.AreEqual(expectedServerResponse, responses);

                await writer.WriteLineAsync("exit"); //signal exit
                await writer.FlushAsync();

            }
        }
    }
}