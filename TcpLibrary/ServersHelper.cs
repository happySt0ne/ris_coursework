using Newtonsoft.Json;

namespace TcpLibrary;

public class ServersHelper {
  public static ServerSettings ReadServers(string path) {
    string json = File.ReadAllText(path);
    ServerSettings settings = JsonConvert
      .DeserializeObject<ServerSettings>(json);

    return settings; 
  }
}
