using System.Net;
using System.Net.Sockets;
using System.Text;
using Matrices;
using Newtonsoft.Json;

namespace TcpLibrary;

public class Servers {
  private List<Thread> _servers = new();
  private readonly object _logLock = new();
  private readonly object _writeLock = new();

  public void ConfigureServers(ServerSettings settings) {
    foreach (var server in settings.Servers) {
      Console.WriteLine(
        $"Starting server at {server.Ip} {server.Port}");

      _servers.Add(new Thread(() =>
        StartServer(server.Ip, server.Port, _tokenSource.Token)));
    }

    _servers.ForEach(s => s.Start());
  }

  private CancellationTokenSource _tokenSource = new();

  private void StartServer(string ip, int port, CancellationToken token) {
    TcpListener server = new(IPAddress.Parse(ip), port);

    server.Start();
    System.Console.WriteLine($"Server started on {ip} {port}");

    while (!token.IsCancellationRequested) {
      TcpClient client = null;

      if (server.Pending()) {
        client = server.AcceptTcpClient();
        System.Console.WriteLine("Client connected");
      } else {
        Thread.Sleep(100);
        continue;
      }
      
      NetworkStream stream = client.GetStream();

      var readedData = ReadData(stream); 
      var a = SendCalculations(stream, readedData);
      PrintLog(stream, a, $"{ip}:{port}");

      client.Close();
    }

    server.Stop();
  }

  private DataTransfer ReadData(NetworkStream stream) {
    byte[] dataLengthBytes = new byte[4];
    stream.Read(dataLengthBytes, 0, 4);
    int dataLength = BitConverter.ToInt32(dataLengthBytes);

    byte[] data = new byte[dataLength];
    stream.Read(data, 0, dataLength);
    string message = Encoding.UTF8.GetString(data, 0, dataLength);

    string[] separetedData = message.Split("||");

    var row = 
      JsonConvert.DeserializeObject<List<Matrix>>(separetedData[0]);
    var blockMatrix = 
      JsonConvert.DeserializeObject<BlockMatrix>(separetedData[1]);

    if (row is null || blockMatrix is null) {
      throw new NullReferenceException(
        "data was not recieved");
    }

    return new(row, blockMatrix);
  }

  private DataTransfer SendCalculations(
      NetworkStream stream, DataTransfer dataTransfer) {

    List<Matrix> result = 
      dataTransfer.Row * dataTransfer.BlockMatrix;
    dataTransfer.Result = result;

    string responce = JsonConvert.SerializeObject(result);
    byte[] data = Encoding.UTF8.GetBytes(responce);

    byte[] dataLengthBytes = BitConverter.GetBytes(data.Length);
    stream.Write(dataLengthBytes, 0, dataLengthBytes.Length);

    stream.Write(data, 0, data.Length);

    return dataTransfer;
  }

  private void PrintLog(NetworkStream stream,
                               DataTransfer dataTransfer,
                               string netPath) {
    lock(_logLock) {
      Console.WriteLine($"Получено на {netPath}:");
      dataTransfer.Row?.ForEach(m => m.ShowMatrix());

      System.Console.WriteLine("Matrix:");
      dataTransfer.BlockMatrix?.ShowMatrix();

      System.Console.WriteLine("MultiplyingResult:");
      dataTransfer.Result?.ForEach(m => m.ShowMatrix());
    }
  }

  public void StopServers() {
    _tokenSource.Cancel();
    _servers.ForEach(s => s.Join());
    
    _servers = new();
  }
}
