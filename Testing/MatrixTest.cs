using Matrices;

[TestClass]
public class MatrixTest {
  [TestMethod]
  public void Test1() {
    Matrix A = new(
      new List<List<double>> () {
        new () {-1, 1},
        new () {-2, 3}
      }
    );
    Matrix B = new(
      new List<List<double>> () {
        new () {-1},
        new () {-2}
      }
    );

    var result = A * B;

    System.Console.WriteLine(new string('-', 20));
    
    result.ShowMatrix();

    Matrix expected = new(
      new List<List<double>> () {
        new () {-1},
        new () {-4}
      }
    );
    
    Assert.AreEqual(expected, result);
  }
}
