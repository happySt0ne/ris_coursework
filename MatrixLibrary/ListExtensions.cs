using Matrices;

namespace ListExt;

public static class ListExtensions {
  public static double MultiplyLists(
      this List<double> first, List<double> second) {
    return Enumerable.Range(0, first.Count)
      .Select(i => first[i] * second[i]).Sum();
  } 

  public static Matrix MultiplyLists(
      this List<Matrix> first, List<Matrix> second) {
    List<Matrix> a = Enumerable.Range(0, first.Count)
      .Select(i => first[i] * second[i]).ToList();

    Matrix sum = new(a[0]);
    for (int i = 1; i < a.Count(); ++i) sum += a[i];

    return sum;
  }

  public static void ShowMatrix(
      this List<List<Matrix>> blockMatrix) {
    for (int i = 0; i < blockMatrix.Count; ++i) {
      List<double> row = new();
      List<double> row2 = new();

      for (int j = 0; j < blockMatrix[0].Count; ++j) {
        row.AddRange(blockMatrix[i][j].GetRow(0));
        row2.AddRange(blockMatrix[i][j].GetRow(1));
      }
      
      row.ForEach(i => System.Console.Write(i + " "));
      System.Console.WriteLine();
      row2.ForEach(i => System.Console.Write(i + " "));
      System.Console.WriteLine();
    }
  }
}

