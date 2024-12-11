using System.Net.Sockets;
using System.Text;
using Matrices;
using Newtonsoft.Json;
using TcpLibrary;

namespace Client;

public class Client { 
  private List<Task<List<Matrix>>> _tasks = new();

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
    _B.ShowMatrix();
    _serializedB = JsonConvert.SerializeObject(_B);
  }

  public void SetMatrixB(List<double> b) {
    _B = new(b);
    _serializedB = JsonConvert.SerializeObject(_B);
  }

  public async Task<List<List<Matrix>>> Start(string pathToServers) {
    var settings = ServersHelper.ReadServers(pathToServers);

    await CreateTasks(settings);
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

    while (_rowNumber != _A.MatrixData.Count) {
      TcpClient client = new();

      await client.ConnectAsync(settings.Servers[serverQueue].Ip,
                                settings.Servers[serverQueue].Port);
      Console.WriteLine($"подключено к серверу" +
                        $"{settings.Servers[serverQueue].Ip}" +
                        $":{settings.Servers[serverQueue].Port}");
      
      _tasks.Add(PushData(client));
      serverQueue++;
      serverQueue %= settings.Servers.Count;
    }
  }

  private async Task<List<Matrix>> PushData(TcpClient client) {
    NetworkStream stream = client.GetStream();

    string jsonToSend = PrepareData();
    await SendData(jsonToSend, stream);
    var result = await ReadResponce(stream);

    client.Close();
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

  private async Task SendData(string jsonToSend,
                              NetworkStream stream) {
    byte[] dataBytes = Encoding.UTF8.GetBytes(jsonToSend);
    
    byte[] dataLengthBytes = BitConverter.GetBytes(dataBytes.Length);
    await stream.WriteAsync(dataLengthBytes, 0, dataLengthBytes.Length);

    System.Console.WriteLine("Данные отправлены");
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
