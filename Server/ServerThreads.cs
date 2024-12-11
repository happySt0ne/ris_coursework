using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using Matrices;
using Newtonsoft.Json;
using TcpLibrary;
using TcpLibrary.DataTypes;
using ThreadArrExtension;

namespace Servers;

public partial class Server {
  private Thread[] _threads = null!;
  private readonly object _logLock = new();

  private TcpClient? _client; 
  private NetworkStream? _stream;

  private BlockMatrix? _B;
  private ConcurrentQueue<List<Matrix>> _data = new();
  private ConcurrentQueue<List<Matrix>> _results = new();

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
      if (_results.TryDequeue(out List<Matrix>? dataToSend)) {
        if (_stream is null) throw new NullReferenceException();
        SendCalculations(_stream, dataToSend);
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
      /*System.Console.WriteLine(_client?.ReceiveBufferSize);*/
      if (_data.TryDequeue(out List<Matrix>? data)) {
        if (_B is null) {
          throw new NullReferenceException("matrix _B must be not null");
        }

        List<Matrix> result = data * _B;

        _results.Enqueue(result);
      }
    }
  }

  private void ReadData() {
    while (!_tokenSource.Token.IsCancellationRequested) {
      if (_client is null && _listener.Pending()) {
        _client = _listener.AcceptTcpClient();
        _stream = _client.GetStream();
        ReadDataFromClient();
        // TODO: тут потом очередь добавь.
      }  

      if (_client is not null) {
        ReadDataFromClient();
      } else {
        Thread.Sleep(100);
        continue;
      }

    }
  }

  private void ReadDataFromClient() {
    byte[] headerBytes = new byte[Header.SizeBytes];
    _stream?.Read(headerBytes, 0, Header.SizeBytes);
    Header header = Header.ConvertFromBytes(headerBytes);

    switch (header.Action) {
      case TcpActions.GetMatrixB:
        ReadMatrixB(header.BodyLength);
        break;
      case TcpActions.GetMatrixA:
        ReadMatrixA(header.BodyLength);
        break;
      case TcpActions.CloseConnection:
        CloseConnection();
        break;
    }
  }

  private void CloseConnection() {
    _B = null;
    _stream?.Close();
    _stream = null;
    _client = null;
  }

  private string ReadMatrix(int bodyLength) {
    int totalRecieved = 0;
    byte[] data = new byte[bodyLength];

    while (totalRecieved < bodyLength) {
      int recivedData = 
        _stream!.Read(data, totalRecieved, bodyLength - totalRecieved);
      if (recivedData == 0) {
        throw new Exception("Соединение прервано!");
      }
      totalRecieved += recivedData;
    }

    string message = Encoding.UTF8.GetString(data, 0, bodyLength);
    return message;
  }

  private void ReadMatrixB(int bodyLength) {
    string message = ReadMatrix(bodyLength);
    _B = JsonConvert.DeserializeObject<BlockMatrix>(message);
  }

  private void ReadMatrixA(int bodyLength) {
    string message = ReadMatrix(bodyLength);
    List<Matrix> row = JsonConvert.DeserializeObject<List<Matrix>>(message)!; 

    _data.Enqueue(row);
  }

  private void SendCalculations(NetworkStream stream,
                                List<Matrix> dataTransfer) {
    /*List<Matrix> result = */
    /*  dataTransfer.Row * dataTransfer.BlockMatrix;*/
    /*dataTransfer.Result = result;*/

    string responce = JsonConvert.SerializeObject(dataTransfer);
    byte[] data = Encoding.UTF8.GetBytes(responce);

    byte[] dataLengthBytes = BitConverter.GetBytes(data.Length);
    stream.Write(dataLengthBytes, 0, dataLengthBytes.Length);

    stream.Write(data, 0, data.Length);
  }
}
