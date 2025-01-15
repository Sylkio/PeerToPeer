using P2PClient.Class;
using System.Net;
using System.Net.Sockets;
namespace P2PClient
{
     public class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== P2P File Transfer Client ===");

            var client = new P2PHandler();

            await client.Start();
        }


    }
}
