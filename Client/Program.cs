using System.Net.Sockets;
using System.Text;
using Matrices;
using network;

public partial class Program {
  private static void Main(string[] args) {
    TcpHelper.ReadIps("../ComputingNodes.txt");

    using (TcpClient client = new(TcpHelper.Ips[0].ip, TcpHelper.Ips[0].port)) {
      System.Console.WriteLine("подключено к серверу");

      NetworkStream stream = client.GetStream();

      string message = "Hello, world!";
      byte[] data = Encoding.UTF8.GetBytes(message);
      
      stream.Write(data, 0, data.Length);

      System.Console.WriteLine("Данные отправлены");
    }
  }
	BlockMatrix z = new(new List<List<double>>() {
    new() {2, 2, 3, 4},
    new() {4, 5, 6, 1},
    new() {2, 3, 4, 5},
    new() {5, 2, 1, 3},
  });
}
