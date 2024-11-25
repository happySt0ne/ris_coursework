using System.Net.Sockets;
using System.Text;
using Matrices;
using Newtonsoft.Json;
using TcpLibrary;

public partial class Program {
  private static List<Task> _tasks = new();
	private static BlockMatrix z = new(new List<List<double>>() {
    new() {2, 2, 3, 4},
    new() {4, 5, 6, 1},
    new() {2, 3, 4, 5},
    new() {5, 2, 1, 3},
  });
  private static int _rowNumber = 0;

  private static async Task Main(string[] args) {
    var settings = ServersHelper.ReadServers("../Servers.json");

    CreateTasks(settings);

    await Task.WhenAll(_tasks);
    System.Console.WriteLine("all tasks completed");
  }

  private static void CreateTasks(ServerSettings settings) {
    foreach (var server in settings.Servers) {
      using (TcpClient client = new(server.Ip, server.Port)) {
        Console.WriteLine($"подключено к серверу {server.Ip}:{server.Port}");

        _tasks.Add(PushData(client));
      }
    }
  }

  private static async Task PushData(TcpClient client) {
    NetworkStream stream = client.GetStream();

    List<Matrix> list = z.GetRow(_rowNumber++);
    System.Console.WriteLine($"list count: {list.Count}");

    string json = JsonConvert.SerializeObject(list);
    System.Console.WriteLine(json);
    byte[] dataBytes = Encoding.UTF8.GetBytes(json);
    
    System.Console.WriteLine("Данные отправлены");
    await stream.WriteAsync(dataBytes, 0, dataBytes.Length);
  }
}
