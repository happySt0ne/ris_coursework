namespace Testing;

public class TestData {
  public List<List<double>> A { get; set; }
  public List<List<double>> B { get; set; }
  public List<List<double>> Result { get; set; }

  public TestData(List<List<double>> a, 
      List<List<double>> b,
      List<List<double>> result) {
    A = a;
    B = b;
    Result = result;
  }
}
