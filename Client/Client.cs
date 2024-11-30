using System.Net.Sockets;
using System.Text;
using Matrices;
using Newtonsoft.Json;
using TcpLibrary;

namespace Client;

public static class Client { 
  private static List<Task<List<Matrix>>> _tasks = new();

	private static BlockMatrix? _A;
  private static BlockMatrix? _B;
    
  private static int _rowNumber = 0;
  private static string? _serializedB;

  public static void SetMatrixA(List<List<double>> a) {
    _A = new(a);
  }

  public static void SetMatrixB(List<List<double>> b) {
    _B = new(b);
    _serializedB = JsonConvert.SerializeObject(_B);
  }

  public static async Task<List<List<Matrix>>> Start(string pathToServers) {
    var settings = ServersHelper.ReadServers(pathToServers);

    await CreateTasks(settings);
    await Task.WhenAll(_tasks);

    List<List<Matrix>> result =
      _tasks.Select(t => t.Result).ToList();

    return result;
  }

  private static async Task CreateTasks(ServerSettings settings) {
    foreach (var server in settings.Servers) {
      TcpClient client = new();

      await client.ConnectAsync(server.Ip, server.Port);
      Console.WriteLine(
        $"подключено к серверу {server.Ip}:{server.Port}");

      _tasks.Add(PushData(client));
    }
  }

  private static async Task<List<Matrix>> PushData(TcpClient client) {
    NetworkStream stream = client.GetStream();

    string jsonToSend = PrepareData();
    await SendData(jsonToSend, stream);
    var result = await ReadResponce(stream);

    client.Close();
    return result;
  }

  private static string PrepareData() {
    if (_A is null) 
      throw new NullReferenceException("matrix A is null");
    if (_B is null)
      throw new NullReferenceException("matrix B is null");

    List<Matrix> list = _A.GetRow(_rowNumber++);
    string rowJson = JsonConvert.SerializeObject(list);
    string jsonToSend = $"{rowJson}||{_serializedB}";

    return jsonToSend;
  }

  private static async Task SendData(
      string jsonToSend, NetworkStream stream) {
    byte[] dataBytes = Encoding.UTF8.GetBytes(jsonToSend);
    
    byte[] dataLengthBytes = BitConverter.GetBytes(dataBytes.Length);
    await stream.WriteAsync(dataLengthBytes, 0, dataLengthBytes.Length);

    System.Console.WriteLine($"dataBytesLeng: {dataBytes.Length}");
    System.Console.WriteLine(string.Join("", dataBytes));
    
    System.Console.WriteLine("Данные отправлены");
    await stream.WriteAsync(dataBytes, 0, dataBytes.Length);
  }

  private static async Task<List<Matrix>> ReadResponce(
      NetworkStream stream) {
    byte[] dataLengthBytes = new byte[4];
    stream.Read(dataLengthBytes, 0, 4);
    int dataLength = BitConverter.ToInt32(dataLengthBytes);

    var data = new byte[dataLength]; 
    await stream.ReadAsync(data, 0, data.Length); 
    string responce = Encoding.UTF8.GetString(data, 0, dataLength);
    
    var result = JsonConvert
      .DeserializeObject<List<Matrix>>(responce);
    if (result is null) {
      throw new NullReferenceException(
        "can't get the result object");
    }

    return result;
  }
}
