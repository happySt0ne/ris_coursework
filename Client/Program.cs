using System.Net;
using System.Net.Sockets;
using System.Text;
using Matrices;

TcpListener listener = null;

IPAddress localAddr = IPAddress.Parse("0.0.0.0");

listener = new(localAddr, 8888);

listener.Start();
System.Console.WriteLine("Сервер запущен");

while (true) {
  TcpClient client = listener.AcceptTcpClient();
  System.Console.WriteLine("Клиент подключен");

  NetworkStream stream = client.GetStream();

  byte[] data = new byte[256];
  int bytes = stream.Read(data, 0, data.Length);
  string message = Encoding.UTF8.GetString(data, 0, bytes);
  Console.WriteLine($"Получено: {message}");

  // Закрытие соединения
  client.Close();
}
