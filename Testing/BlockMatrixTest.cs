using Matrices;

[TestClass]
public class BlockMatrixTest {
  [TestMethod]
  public void Test1() {
    BlockMatrix first = new(
      new List<List<double>> {
        new() {1, -1, 3, 1},
        new() {4, -1, 5, 4},
        new() {2, -2, 4, 1},
        new() {1, -4, 5, -1},
      }
    );

    BlockMatrix second = new(new List<double> {5, 4, 6, 3});

    var result = first * second;
    BlockMatrix expected = new(new List<double> {22, 58, 29, 16});
    
    Assert.AreEqual(expected, result);
  }

  [TestMethod]
  public void SubstructionTest() {
    BlockMatrix A = new(
      new List<List<double>> {
        new() {3, 2, 3, 4},
        new() {1, 2, 7, 8},
        new() {2, 21, 7, 2},
        new() {3, 4, -23, 0},
      }
    );

    BlockMatrix B = new(
      new List<List<double>> {
        new() {2, 1, 7, 8},
        new() {5, 2, 2, 3},
        new() {45, 22, 1, 4},
        new() {0, -2, 1, -2},
      }
    );

    BlockMatrix result = A - B;
    BlockMatrix expected = new(
      new List<List<double>> {
        new() {1, 1, -4, -4},
        new() {-4, 0, 5, 5},
        new() {-43, -1, 6, -2},
        new() {3, 6, -24, 2},
      }
    );

    Assert.AreEqual(expected, result);
  }

  [TestMethod]
  public void SubstructionVectorsTest() {
    BlockMatrix A = new(
      new List<double> {4, 2, 2, 5}
    ); 

    BlockMatrix B = new(
      new List<double> {1, 6, 3, 2}
    ); 

    BlockMatrix result = A - B;
    BlockMatrix expected = new(
      new List<double> {3, -4, -1, 3}
    );

    Assert.AreEqual(expected, result);
  }
}
