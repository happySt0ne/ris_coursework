using System.Net;
using System.Net.Sockets;
using TcpLibrary;
using ThreadArrExtension;

namespace Servers;

public partial class Server : IDisposable {
  private bool _disposed = false;
  private TcpListener _listener;
  private CancellationTokenSource _tokenSource = new();
  private ServerInfo? _address;

  public ServerInfo Address { 
    get {
      if (_address is null) throw new NullReferenceException();
      return _address;
    }
    private init {
      _address = value;
    }
  }

  public Server(ServerInfo info) {
    var (port, ip) = info;
    _listener = new(ip, port);
    
    StartThreads();
  }

  /*public Server(string pathToFile) {*/
  /*  System.Console.WriteLine(pathToFile);*/
  /**/
  /*  var settings = ServersHelper.ReadServers(pathToFile);*/
  /*  _address = ServersHelper.FirstFreeAddress(settings); */
  /**/
  /*  _listener = ServersHelper.StartListener(_address);*/
  /**/
  /*  System.Console.WriteLine($"Server started at: {_address}");*/
  /**/
  /*  StartThreads();*/
  /*}*/

  public Server(string pathToFile) {
    var Address = ServersHelper.GetMyAddress();
    var (port, ip) = Address;

    _listener = new(ip, port);
    _listener.Start();

    System.Console.WriteLine($"server started at: {Address}");

    var settings = ServersHelper.ReadServers(pathToFile);
    settings.Add(Address);

    ServersHelper.WriteSettings(pathToFile, settings);

    StartThreads();
  }

  ~Server() {
    Dispose(false);
  }
  
  public void Dispose() {
    Dispose(true); 
    GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing) {
    if (_disposed) return;
    if (disposing) {
      _threads.JoinAll();
      _threads = null!;
      _B = null;
      _tokenSource.Cancel();
    }
    
    _listener?.Dispose();
    _disposed = true;
  }
}
