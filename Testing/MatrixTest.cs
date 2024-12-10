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
      new List<List<double>> {
        new () {-1},
        new () {-4}
      }
    );
    
    Assert.AreEqual(expected, result);
  }

  [TestMethod]
  public void SubstructionTest() {
    Matrix A = new(
      new List<List<double>> {
        new() {3, 2},
        new() {1, 2}
      }
    );

    Matrix B = new(
      new List<List<double>> {
        new() {2, 1},
        new() {5, 2}
      }
    );

    Matrix result = A - B;
    Matrix expected = new(
      new List<List<double>> {
        new() {1, 1},
        new() {-4, 0}
      }
    );

    Assert.AreEqual(expected, result);
  }

  [TestMethod]
  public void SubstrTestWithVector() {
    Matrix A = new(
      new List<List<double>> {
        new() {3},
        new() {1},
      }
    );

    Matrix B = new(
      new List<List<double>> {
        new() {1},
        new() {2},
      }
    );

    Matrix result = A - B;
    Matrix expected = new(
      new List<List<double>> {
        new() {2},
        new() {-1},
      }
    );

    Assert.AreEqual(expected, result);
  }
}
