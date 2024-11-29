using System.Net;
using System.Net.Sockets;
using System.Text;
using Matrices;
using Newtonsoft.Json;

namespace TcpLibrary;

public class Servers {
  private static List<Thread> _servers = new();
  private static readonly object _logLock = new();
  private static readonly object _writeLock = new();

  public static void ConfigureServers(ServerSettings settings) {
    foreach (var server in settings.Servers) {
      Console.WriteLine(
        $"Starting server at {server.Ip} {server.Port}");

      _servers.Add(new Thread(() =>
        StartServer(server.Ip, server.Port)));
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

      var readedData = ReadData(stream); 
      var a = SendCalculations(stream, readedData);
      PrintLog(stream, a, $"{ip}:{port}");

      client.Close();
    }
  }

  private static DataTransfer ReadData(NetworkStream stream) {
    byte[] data = new byte[256];

    int bytes = stream.Read(data, 0, data.Length);
    string message = Encoding.UTF8.GetString(data, 0, bytes);

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

  private static DataTransfer SendCalculations(
      NetworkStream stream, DataTransfer dataTransfer) {
    var data = new byte[256];

    List<Matrix> result = 
      dataTransfer.Row * dataTransfer.BlockMatrix;
    dataTransfer.Result = result;

    string responce = JsonConvert.SerializeObject(result);
    data = Encoding.UTF8.GetBytes(responce);

    lock(_writeLock) {
      stream.Write(data, 0, data.Length);
    }

    return dataTransfer;
  }

  private static void PrintLog(NetworkStream stream,
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
}
