using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using Matrices;
using Newtonsoft.Json;

namespace TcpLibrary;

public class ServersHelper {
  private static Random _random;
  private static readonly object _writeFileLock;

  static ServersHelper() {
    _random = new(Guid.NewGuid().GetHashCode());
    _writeFileLock = new();
  }

  public static ServerSettings ReadServers(string path) {
    string json = File.ReadAllText(path);
    ServerSettings? settings = JsonConvert
      .DeserializeObject<ServerSettings>(json);
    
    if (settings is null) 
      throw new NullReferenceException("settings is null");

    return settings; 
  }

  public static void WriteSettings(string pathToFile, ServerSettings settings) {
    var serialisedSettings = JsonConvert.SerializeObject(settings);
    
    lock(_writeFileLock) using (StreamWriter sw = new(pathToFile)) {
      sw.Write(serialisedSettings);
    } 
  }

  public static void DeleteAddress(ServerInfo info, string pathToFile) {

  }

  public static bool IsPortInUse(int port) {
    var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();  
    List<IPEndPoint> endPoints = ipGlobalProperties.GetActiveTcpListeners()
      .Union(ipGlobalProperties.GetActiveTcpListeners()).ToList();

    return endPoints.Where(i => i.Port == port).ToList().Count > 0;
  }

  public static ServerInfo GetMyAddress() {
    var ipAddress = Dns.GetHostAddresses(Dns.GetHostName()).Where(a => 
      a.AddressFamily is AddressFamily.InterNetwork).First();
    int port;

    do {
      port = _random.Next(8000, 9000);
    } while(IsPortInUse(port));
    
    return new(ipAddress.ToString(), port);
  }

  public static ServerInfo FirstFreeAddress(ServerSettings settings) {
    throw new NotImplementedException();
  }

  private DataTransfer SendCalculations(NetworkStream stream,
                                        DataTransfer dataTransfer) {
    List<Matrix> result = 
      dataTransfer.Row * dataTransfer.BlockMatrix;
    dataTransfer.Result = result;

    string responce = JsonConvert.SerializeObject(result);
    byte[] data = Encoding.UTF8.GetBytes(responce);

    byte[] dataLengthBytes = BitConverter.GetBytes(data.Length);
    stream.Write(dataLengthBytes, 0, dataLengthBytes.Length);

    stream.Write(data, 0, data.Length);

    return dataTransfer;
  }

  private DataTransfer ReadData(NetworkStream stream) {
    byte[] dataLengthBytes = new byte[4];
    stream.Read(dataLengthBytes, 0, 4);
    int dataLength = BitConverter.ToInt32(dataLengthBytes);

    byte[] data = new byte[dataLength];
    stream.Read(data, 0, dataLength);
    string message = Encoding.UTF8.GetString(data, 0, dataLength);

    string[] separetedData = message.Split("||");

    var row = 
      JsonConvert.DeserializeObject<List<Matrix>>(separetedData[0]);
    var blockMatrix = 
      JsonConvert.DeserializeObject<BlockMatrix>(separetedData[1]);

    if (row is null || blockMatrix is null) {
      throw new NullReferenceException(
        "data was not recieved");
    }

    return new(row, blockMatrix);
  }
}
