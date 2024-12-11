using System.Net.Sockets;
using System.Text;
using Matrices;
using Newtonsoft.Json;
using TcpLibrary;

namespace Client;

public class Client { 
  private List<Task<List<Matrix>>> _tasks = new();
  private List<TcpClient> _clients = new();
  private List<NetworkStream> _streams = new();

	private BlockMatrix? _A;
  private BlockMatrix? _B;
   
  private int _rowNumber = 0;
  private string? _serializedB;

  public void SetMatrixA(List<List<double>> a) {
    _A = new(a);
  }

  public void SetMatrixB(List<List<double>> b) {
    _B = new(b);
    _serializedB = JsonConvert.SerializeObject(_B);
  }

  public void SetMatrixB(double[,] b) {
    _B = new(b);
    _serializedB = JsonConvert.SerializeObject(_B);
  }

  public void SetMatrixB(List<double> b) {
    _B = new(b);
    _serializedB = JsonConvert.SerializeObject(_B);
  }

  private ServerSettings? _settings;

  public async Task<List<List<Matrix>>> Start(string pathToServers) {
    _settings = ServersHelper.ReadServers(pathToServers);

    await CreateTasks(_settings);
    await Task.WhenAll(_tasks);

    List<List<Matrix>> result =
      _tasks.Select(t => t.Result).ToList();

    return result;
  }

  private async Task CreateTasks(ServerSettings settings) {
    if (_A is null) {
      throw new NullReferenceException("A matrix is null!");
    }

    int serverQueue = 0;

    foreach (ServerInfo? server in settings.Servers) {
      TcpClient client = new();

      await client.ConnectAsync(server.Ip, server.Port);
      Console.WriteLine($"подключено к серверу" +
                        $"{server.Ip}:{server.Port}");

      NetworkStream stream = client.GetStream();

      _streams.Add(stream);
      _clients.Add(client);
    }
    
    while (_rowNumber != _A.MatrixData.Count) {
      _tasks.Add(PushData(serverQueue));
      serverQueue++;
      serverQueue %= settings.Servers.Count;
    }
  }

  private async Task<List<Matrix>> PushData(int queueIndex) {
    string jsonToSend = PrepareData();
    await SendData(jsonToSend, queueIndex);
    var result = await ReadResponce(_streams[queueIndex]);

    return result;
  }

  private string PrepareData() {
    if (_A is null) 
      throw new NullReferenceException("matrix A is null");
    if (_B is null)
      throw new NullReferenceException("matrix B is null");

    List<Matrix> list = _A.GetRow(_rowNumber++);
    string rowJson = JsonConvert.SerializeObject(list);
    string jsonToSend = $"{rowJson}||{_serializedB}";

    return jsonToSend;
  }

  private async Task SendData(string jsonToSend, int index) {
    NetworkStream stream = _streams[index];
    byte[] dataBytes = Encoding.UTF8.GetBytes(jsonToSend);
    
    byte[] dataLengthBytes = BitConverter.GetBytes(dataBytes.Length);
    await stream.WriteAsync(dataLengthBytes, 0, dataLengthBytes.Length);

    System.Console.WriteLine($"Данные отправлены на сервер {_settings?.Servers[index]}");
    await stream.WriteAsync(dataBytes, 0, dataBytes.Length);
  }

  private async Task<List<Matrix>> ReadResponce(
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
