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
}

