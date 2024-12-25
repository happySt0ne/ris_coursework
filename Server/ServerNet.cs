using System.Net.Sockets;
using TcpLibrary;
using ThreadArrExtension;

namespace Servers;

public partial class Server : IDisposable {
  private bool _disposed = false;
  private TcpListener _listener;
  private CancellationTokenSource _tokenSource = new();
  private ServerInfo? _address;
  private string? _pathToAddressesFile;

  public ServerInfo Address { 
    get {
      if (_address is null) throw new NullReferenceException();
      return _address;
    }
    private init {
      _address = value;
    }
  }

  public Server() {
    Address = ServersHelper.GetMyAddress();
    var (port, ip) = Address;

    _listener = new(ip, port);
  }

  public Server(string pathToFile) : this() {
    _pathToAddressesFile = pathToFile;

    var settings = ServersHelper.ReadServers(pathToFile);
    settings.Add(Address);

    ServersHelper.WriteSettings(pathToFile, settings);
  }
  
  public void Start() {
    _listener.Start(3);

    System.Console.WriteLine($"server started at: {Address}");

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
      _tokenSource.Cancel();
      _threads.JoinAll();
      _threads = null!;
      _B = null;
    }

    if (_pathToAddressesFile is not null) {
      ServersHelper.DeleteAddress(Address, _pathToAddressesFile);
    }

    _listener?.Dispose();
    _disposed = true;
  }
}
