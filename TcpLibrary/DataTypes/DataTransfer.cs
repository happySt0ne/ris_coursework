
using Matrices;

namespace TcpLibrary;

public class DataTransfer {
  public List<Matrix> Row { get; init; }
  public BlockMatrix BlockMatrix { get; init; }

  public List<Matrix>? Result { get; set; }

  public DataTransfer(List<Matrix> row, BlockMatrix blockMatrix) {
    Row = row;
    BlockMatrix = blockMatrix;
  }
}
