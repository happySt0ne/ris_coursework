using System.Net;
using System.Net.Sockets;
using System.Text;
using Matrices;
using Newtonsoft.Json;

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

  private static readonly object _lockObj = new();

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

      string[] separetedData = message.Split("||");

      var row = JsonConvert.DeserializeObject<List<Matrix>>(separetedData[0]);
      var blockMatrix = JsonConvert.DeserializeObject<BlockMatrix>(separetedData[1]);

      if (row is null || blockMatrix is null) {
        throw new NullReferenceException(
          "data was not recieved");
      }

      Array.Clear(data, 0, data.Length);

      List<Matrix> result = row * blockMatrix;
      string responce = JsonConvert.SerializeObject(result);
      data = Encoding.UTF8.GetBytes(responce);

      lock(_lockObj) {
        Console.WriteLine($"Получено на {port}:");
        row?.ForEach(m => m.ShowMatrix());

        System.Console.WriteLine("Matrix:");
        blockMatrix?.ShowMatrix();

        System.Console.WriteLine("MultiplyingResult:");
        result.ForEach(m => m.ShowMatrix());

        stream.Write(data, 0, data.Length);
      }

      stream.Close();
      client.Close();
    }
  }
}
