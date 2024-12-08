using System.Net;

namespace TcpLibrary;

public class ServerInfo {
  public string Ip { get; set; }
  public int Port { get; set; }

  public ServerInfo(string ip, int port) {
    Ip = ip;
    Port = port;
  }

  public ServerInfo(IPEndPoint? endPoint) {
    if (endPoint is null) throw new NullReferenceException();

    Ip = endPoint.Address.ToString();
    Port = endPoint.Port;
  }

  public ServerInfo() { }

  public override string ToString() {
    return $"{Ip}:{Port}";
  }

  public void Deconstruct(out int port, out IPAddress address) {
    port = Port;
    address = IPAddress.Parse(Ip);
  }

  public override bool Equals(object? obj) {
    if (obj is ServerInfo info) {
      return info.Ip == this.Ip && info.Port == this.Port;
    }

    return false;
  }

  public override int GetHashCode() => base.GetHashCode(); 
}

