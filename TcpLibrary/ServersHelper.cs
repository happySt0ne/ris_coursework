using Newtonsoft.Json;

namespace TcpLibrary;

public class ServersHelper {
  public static ServerSettings ReadServers(string path) {
    string json = File.ReadAllText("../Servers.json");
    ServerSettings settings = JsonConvert
      .DeserializeObject<ServerSettings>(json);

    return settings; 
  }
}
