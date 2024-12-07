using System.Net;
using System.Net.Sockets;
using TcpLibrary;

namespace Testing;

[TestClass]
public class UnitTest2 {

  [TestMethod]
  public void TestStuff() {
    TcpListener listener = new(IPAddress.Any, 8888);
    listener.Start();

    var result = ServersHelper.IsPortInUse(8888); 

    Assert.AreEqual(true, result);
    listener.Stop();
  }
}
