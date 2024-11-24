namespace network;

using Ip = (string ip, int port);

public class TcpHelper {
  public static List<Ip> ips { get; } = new();

  public static void ReadIps(string path) {
    using (StreamReader reader = new(path)) {
      string? line;

      while ((line = reader.ReadLine()) != null) {
        string[] splittedLine = line.Split(':');

        string ip = splittedLine[0];
        string port = splittedLine[1];

        ips.Add((ip, int.Parse(port)));
      }
    }
  }
}
