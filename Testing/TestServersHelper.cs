using System.Net;
using System.Net.Sockets;
using TcpLibrary;

namespace Testing;

[TestClass]
public class TestServersHelper {

  [TestMethod]
  public void TestIsPortInUse() {
    TcpListener listener = new(IPAddress.Any, 8888);
    listener.Start();

    var result = ServersHelper.IsPortInUse(8888); 

    Assert.AreEqual(true, result);
    listener.Stop();
  }

  [TestMethod]
  public void TestDeleteAddress() {
    ServerSettings settings = new();
    ServerInfo info = new ("222", 222);

    var a = ServersHelper.ReadServers("TestFile.json");

    settings.Add(info);
    ServersHelper.WriteSettings("TestFile.json", settings);

    ServersHelper.DeleteAddress(info, "TestFile.json");
    
    var b = ServersHelper.ReadServers("TestFile.json");

    Assert.AreEqual(a, b);
  } 
}
