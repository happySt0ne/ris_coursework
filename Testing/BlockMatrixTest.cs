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
}
