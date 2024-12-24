using Matrices;
using Numerics = MathNet.Numerics.LinearAlgebra;

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

  public static double[,] convertToArr(this List<List<double>> list) {
    double[,] result = new double[list.Count, list.Count];

    for (int i = 0; i < list.Count; ++i) {
      for (int j = 0; j < list[0].Count; ++j) {
        result[i, j] = list[i][j];
      }
    }

    return result;
  }

  public static void ShowMatrix(this List<List<Matrix>> blockMatrix,
                                bool isUneven = true) {
    for (int i = 0; i < blockMatrix.Count; ++i) {
      List<double> row = new();
      List<double> row2 = new();

      for (int j = 0; j < blockMatrix[0].Count; ++j) {
        row.AddRange(blockMatrix[i][j].GetRow(0));
        row2.AddRange(blockMatrix[i][j].GetRow(1));
      }
      
      row.ForEach(i => System.Console.Write(i + " "));
      System.Console.WriteLine();

      if (isUneven && blockMatrix.Count - 1 == i ) return;

      row2.ForEach(i => System.Console.Write(i + " "));
      System.Console.WriteLine();
    }
  }

  public static List<T> Exact<T> (this List<T> list, in int colIndex) {
    List<T> result = new();

    for (int i = 0; i < list.Count; ++i) {
      if (i == colIndex) continue;
      
      result.Add(list[i]);
    } 

    return result;
  }

  public static List<Matrix> Plus(this List<Matrix> first, List<Matrix> second) {
    List<Matrix> result = new();

    for (int i = 0; i < first.Count; ++i) {
      result.Add(first[i] + second[i]);
    }
    
    return result;
  }

  public static List<Matrix> Substract(this List<Matrix> first, List<Matrix> second) {
    List<Matrix> result = new();

    for (int i = 0; i < first.Count; ++i) {
      result.Add(first[i] - second[i]);
    }

    return result;
  } 

  public static List<Matrix> MultiplyAsLists(this List<Matrix> first, List<Matrix> second) {
    List<Matrix> result = new();

    for (int i = 0; i < first.Count; ++i) {
      result.Add(first[i]*second[i]);
    }

    return result;
  }

  public static List<Matrix> DivideBy(this List<Matrix> list, Matrix matrix) {
    List<Matrix> result = new();

    foreach (Matrix m in list) {
      result.Add(m / matrix);
    }

    return result;
  }
}

