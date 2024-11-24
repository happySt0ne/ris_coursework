using System.Net.Sockets;
using System.Text;
using network;

public partial class Program {
  private static void Main(string[] args) {
    TcpHelper.ReadIps("../ComputingNodes.txt");

    TcpHelper.ips.ForEach(i => Console.WriteLine($"{i.port} {i.ip}"));

    using (TcpClient client = new(TcpHelper.ips[0].ip, TcpHelper.ips[0].port)) {
      System.Console.WriteLine("подключено к серверу");

      NetworkStream stream = client.GetStream();

      string message = "Hello, world!";
      byte[] data = Encoding.UTF8.GetBytes(message);
      
      stream.Write(data, 0, data.Length);

      System.Console.WriteLine("Данные отправлены");
    }
  }
}
