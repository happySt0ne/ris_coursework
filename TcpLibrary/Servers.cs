using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpLibrary;

public class Servers {
  private static List<Thread> _servers = new();

  public static void ConfigureServers(ServerSettings settings) {
    foreach (var server in settings.Servers) {
      Console.WriteLine($"Starting server at {server.Ip} {server.Port}");

      _servers.Add(new Thread(() => StartServer(server.Ip, server.Port)));
    }

    _servers.ForEach(s => s.Start());
  }

  private static void StartServer(string ip, int port) {
    TcpListener server = new(IPAddress.Parse(ip), port);

    server.Start();
    System.Console.WriteLine($"Server started on {ip} {port}");

    while (true) {
      TcpClient client = server.AcceptTcpClient();
      System.Console.WriteLine("Client connected");

      NetworkStream stream = client.GetStream();

      byte[] data = new byte[256];
      int bytes = stream.Read(data, 0, data.Length);
      string message = Encoding.UTF8.GetString(data, 0, bytes);
      Console.WriteLine($"Получено на {port}: {message}");

      client.Close();
    }
  }
}
