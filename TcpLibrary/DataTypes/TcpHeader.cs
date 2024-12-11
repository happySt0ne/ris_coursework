using Newtonsoft.Json;
using System.Runtime;

namespace TcpLibrary.DataTypes;

public struct Header {
  public TcpActions Action { get; init; }
  public int BodyLength { get; init; }

  public const int SizeBytes = 8;

  public Header (TcpActions action, int bodyLength) {
    BodyLength = bodyLength;
    Action = action;
  }

  public byte[] ConvertToBytes() {
    byte[] bytes = new byte[8];

    var a = BitConverter.GetBytes((int)Action);
    for (int i = 0, j = 0; i < 4; ++i, ++j) {
      bytes[i] = a[j];
    }

    var b = BitConverter.GetBytes(BodyLength);
    for (int i = 4, j = 0; i < 8; ++i, ++j) {
      bytes[i] = b[j];
    }

    return bytes;
  }

  public static Header ConvertFromBytes(byte[] bytes) {
    var action = (TcpActions)BitConverter.ToInt32(bytes, 0);
    int bodyLength = BitConverter.ToInt32(bytes, 4);

    return new(action, bodyLength);
  }
  
  public override string ToString() {
    return $"{Action} {BodyLength}";
  }
}

