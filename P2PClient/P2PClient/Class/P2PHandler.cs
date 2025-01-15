using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace P2PClient.Class
{
    public class P2PHandler
    {
        private string _myIp;
        private int _myPort;

        private TcpListener _listener;

        public async Task Start()
        {
            //Connection 
            SetupLocalConnection();

            while (true)
            {
                ShowMenu();
                var choice = Console.ReadLine();
                await HandleChoice(choice);
            }
        }

        private void SetupLocalConnection()
        {
            _myIp = GetLocalIPAddress();

            _myPort = 13000;

            Console.WriteLine($"Current adress: {_myIp}:{_myPort}");

            _listener = new TcpListener(IPAddress.Parse(_myIp), _myPort);
            _listener.Start();
        }

        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            throw new Exception("No network adapter");
        }

        private void ShowMenu()
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Connect to peer");
            Console.WriteLine("2. Wait for connection");
            Console.WriteLine("3. Exit");
        }

        private async Task HandleChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    await ConnectToPeer();
                    break;
                case "2":
                    await WaitForConnection();
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }

        }

        private async Task ConnectToPeer()
        {
            Console.WriteLine("Enter peer IP: ");
            var peerIp = Console.ReadLine();
            Console.WriteLine("Enter peer port: ");
            var peerPort = int.Parse(Console.ReadLine());

            try
            {
                var client = new TcpClient();
                await client.ConnectAsync(peerIp, peerPort);
                Console.WriteLine("Connected to peer");

            }

            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect: {ex.Message}");
            }
        }

        private async Task WaitForConnection()
        {
            Console.WriteLine("Waiting for peer to connect...");
            var client = await _listener.AcceptTcpClientAsync();
            Console.WriteLine("Connected");
        }

    }
}
