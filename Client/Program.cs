using System.Net.Sockets;
using System.Text;
using Matrices;
using TcpLibrary;

public partial class Program {
  private static List<Task> _tasks = new();

  private static async Task Main(string[] args) {
    var settings = ServersHelper.ReadServers("../Servers.json");

    foreach (var server in settings.Servers) {
      using (TcpClient client = new(server.Ip, server.Port)) {
        Console.WriteLine($"подключено к серверу {server.Ip}:{server.Port}");

        _tasks.Add(PushData(client));
      }
    }

    await Task.WhenAll(_tasks);
    System.Console.WriteLine("all tasks completed");
  }

  private static async Task PushData(TcpClient client) {
      NetworkStream stream = client.GetStream();

      string message = "Hello, world!";
      byte[] data = Encoding.UTF8.GetBytes(message);
      
      stream.Write(data, 0, data.Length);

      System.Console.WriteLine("Данные отправлены");
  }

	BlockMatrix z = new(new List<List<double>>() {
    new() {2, 2, 3, 4},
    new() {4, 5, 6, 1},
    new() {2, 3, 4, 5},
    new() {5, 2, 1, 3},
  });
}
