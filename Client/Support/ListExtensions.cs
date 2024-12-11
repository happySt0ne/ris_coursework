using Numerics = MathNet.Numerics.LinearAlgebra;

namespace Client.Extensions;

static class ListExtensions {
  public static double[,] CastToArray(this List<List<double>> list) {
    double[,] result = new double[list.Count, list[0].Count];

    for (int i = 0; i < list.Count; ++i) {
      for (int j = 0; j < list[0].Count; ++j) {
        result[i, j] = list[i][j];
      }
    }

    return result;
  }

  public static List<List<double>> ConvertMatrixToList(
     this Numerics.Matrix<double> matrix) {
    List<List<double>> nestedList = new List<List<double>>();

    for (int i = 0; i < matrix.RowCount; i++) {
      List<double> row = new List<double>();
        
      for (int j = 0; j < matrix.ColumnCount; j++) {
        row.Add(matrix[i, j]);
      }
      nestedList.Add(row);
    }

    return nestedList;
  }
}
