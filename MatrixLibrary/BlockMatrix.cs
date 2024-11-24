using ListExt;

namespace Matrices;

public class BlockMatrix {
  private List<List<Matrix>> _matrix { get; }

  public BlockMatrix(List<List<double>> matrix) {
    List<Matrix> matrices = new();
    _matrix = new();

    for (int i = 0; i < matrix.Count; i += 2) {
      for (int j = 0; j < matrix[0].Count; j += 2) {
        List<List<double>> z = new();

        z.Add(new List<double> {matrix[i][j], matrix[i][j+1]});
        z.Add(new List<double> {matrix[i+1][j], matrix[i+1][j+1]});

        matrices.Add(new(z));
      }
    }

    for (int i = 0; i < matrices.Count; i += 2) {
      _matrix.Add(matrices[i..(i+2)]);
    }
  }

  public BlockMatrix(List<List<Matrix>> matrix) {
    _matrix = matrix;
  }

  public void ShowMatrix() {
    for (int i = 0; i < _matrix.Count; ++i) {
      List<double> row = new();
      List<double> row2 = new();

      for (int j = 0; j < _matrix[0].Count; ++j) {
        row.AddRange(_matrix[i][j].GetRow(0));
        row2.AddRange(_matrix[i][j].GetRow(1));
      }
      
      row.ForEach(i => System.Console.Write(i + " "));
      System.Console.WriteLine();
      row2.ForEach(i => System.Console.Write(i + " "));
      System.Console.WriteLine();
    }
  }

  public List<Matrix> GetRow(int rowIndex)
    => _matrix[rowIndex];
  
  public List<Matrix> GetColumn(int colIndex) 
    => _matrix.Select(row => row[colIndex]).ToList();

  public static BlockMatrix operator *(
      BlockMatrix A, BlockMatrix B) {
    List<List<Matrix>> result = new(); 

    for (int i = 0; i < A._matrix.Count; ++i) {
      List<Matrix> row = new();

      for (int j = 0; j < A._matrix[0].Count; ++j) {
        row.Add(A.GetRow(i).MultiplyLists(B.GetColumn(j)));
      }

      result.Add(row);
    }    

    return new(result);
  }
}
