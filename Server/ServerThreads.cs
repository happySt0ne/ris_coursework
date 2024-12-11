using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using Matrices;
using Newtonsoft.Json;
using TcpLibrary;
using ThreadArrExtension;

namespace Servers;

public partial class Server {
  private Thread[] _threads = null!;
  private readonly object _logLock = new();
  private ConcurrentQueue<DataTransfer> _data = new();
  private ConcurrentQueue<DataTransfer> _results = new();

  private NetworkStream? _stream;
  private BlockMatrix? _B;

  private void StartThreads() {
    _threads = new Thread[3] {
      new(() => ReadData()),
      new(() => Calculate()),
      new(() => SendData())
    };

    _threads.StartAll();
  }

  private void SendData() {
    while (!_tokenSource.Token.IsCancellationRequested) {
      if (_results.TryDequeue(out DataTransfer? dataToSend)) {
        if (_stream is null) throw new NullReferenceException();
        var a = SendCalculations(_stream, dataToSend);
        
        if (_address is null) throw new NullReferenceException();
        PrintLog(_stream, a, _address.ToString());
      }
    } 
  }

  private void PrintLog(NetworkStream stream,
                        DataTransfer dataTransfer,
                        string netPath) {
    lock(_logLock) {
      Console.WriteLine($"Получено на {netPath}:");
      dataTransfer.Row?.ForEach(m => m.ShowMatrix());

      System.Console.WriteLine("Matrix:");
      dataTransfer.BlockMatrix?.ShowMatrix();

      System.Console.WriteLine("MultiplyingResult:");
      dataTransfer.Result?.ForEach(m => m.ShowMatrix());
    }
  }

  private void Calculate() {
    while (!_tokenSource.Token.IsCancellationRequested) {
      if (_data.TryDequeue(out DataTransfer? data)) {
        data.Result = data.Row * data.BlockMatrix;

        _results.Enqueue(data);
      }
    }
  }
  
  private void ReadData() {
    TcpClient client = null!;

    while (!_tokenSource.Token.IsCancellationRequested) {
      if (_listener.Pending()) {
        client = _listener.AcceptTcpClient();
      } else {
        Thread.Sleep(100);
        continue;
      }

      _stream = client.GetStream();

      var readedData = ReadData(_stream);
      _data.Enqueue(readedData);
    }
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
}
