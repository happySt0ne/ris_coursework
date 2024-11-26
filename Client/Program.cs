using System.Net.Sockets;
using System.Text;
using Matrices;
using Newtonsoft.Json;
using TcpLibrary;
using ListExt;

public partial class Program {
  private static List<Task<List<Matrix>>> _tasks = new();
	private static BlockMatrix _A 
      = new(new List<List<double>>() {
    new() {2, 2, 3, 4},
    new() {4, 5, 6, 1},
    new() {2, 3, 4, 5},
    new() {5, 2, 1, 3},
  });
  private static BlockMatrix _B 
      = new(new List<List<double>>() {
    new() {7, 8, 2, 3},
    new() {9, 3, 4, 1},
    new() {2, 3, 3, 4},
    new() {5, 1, 2, 6},
  });
  private static int _rowNumber = 0;

  private static async Task Main(string[] args) {
    var settings = ServersHelper.ReadServers("../Servers.json");

    CreateTasks(settings);
    await Task.WhenAll(_tasks);

    System.Console.WriteLine("Result of multiplying:");
    List<List<Matrix>> result =
      _tasks.Select(t => t.Result).ToList();

    result.ShowMatrix();
  }

  private static void CreateTasks(ServerSettings settings) {
    foreach (var server in settings.Servers) {
      using (TcpClient client = new(server.Ip, server.Port)) {
        Console.WriteLine($"подключено к серверу {server.Ip}:{server.Port}");

        _tasks.Add(PushData(client));
      }
    }
  }

  private static async Task<List<Matrix>> PushData(TcpClient client) {
    NetworkStream stream = client.GetStream();

    List<Matrix> list = _A.GetRow(_rowNumber++);

    string rowJson = JsonConvert.SerializeObject(list);
    string matrixJson = JsonConvert.SerializeObject(_B); // вот эту хуйню можно вынести отдельно из цикла что б не повторять.

    string jsonResult = $"{rowJson}||{matrixJson}";

    byte[] dataBytes = Encoding.UTF8.GetBytes(jsonResult);
    
    System.Console.WriteLine("Данные отправлены");
    await stream.WriteAsync(dataBytes, 0, dataBytes.Length);

    Array.Clear(dataBytes, 0, dataBytes.Length);

    stream.Read(dataBytes, 0, dataBytes.Length); // Вот здесь нужно ебнуть Asynс чтобы нормально было
    string responce = Encoding.UTF8
      .GetString(dataBytes, 0, dataBytes.Length);
    var z = JsonConvert.DeserializeObject<List<Matrix>>(responce);
    if (z is null) throw new NullReferenceException();

    return z;
  }
}
