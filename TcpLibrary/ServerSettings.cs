namespace TcpLibrary;

public class ServerSettings {
  public List<ServerInfo> Servers { get; set; } = null!;

  public ServerSettings() {}

  public ServerInfo this[int index] {
    get { 
      if (Servers is null) throw new NullReferenceException(); 
      return Servers[index];
    } 
  }

  public void Add(ServerInfo info) {
    if (Servers is null) throw new NullReferenceException();
    Servers.Add(info);
  }
}
