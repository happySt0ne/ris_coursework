namespace TcpLibrary;

public class ServerSettings {
  public List<ServerInfo> Servers { get; set; }

  public ServerSettings() {
    Servers = new();
  }

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

  public override string ToString() {
    var result = string.Empty;

    foreach (var item in Servers) {
      result += item.ToString() + '\n';
    }

    return result;
  }

  public override int GetHashCode() => base.GetHashCode();

  public override bool Equals(object? obj) {
    if (obj is null) return false;
    if (obj is not ServerSettings) return false;

    var settings = obj as ServerSettings;
    if (settings is null) return false;

    if (settings.Servers.Count != Servers.Count) return false;

    bool result = true;
    for (int i = 0; i < settings.Servers.Count; ++i) {
      result = result && (this[i].Equals(settings[i]));
    }

    return result;
  }
}
