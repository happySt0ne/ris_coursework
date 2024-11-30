using Matrices;
using TcpLibrary;

namespace Testing;

using Client = Client.Client;

[TestClass]
public class UnitTest1 {
  private string pathToServersIp = "../../../../Servers.json";

  public static IEnumerable<object[]> GetDataList() {
    var dataList = new List<TestData> {
      new (
        new List<List<double>> {
          new() {2, 2, 3, 4},
          new() {4, 5, 6, 1},
          new() {2, 3, 4, 5},
          new() {5, 2, 1, 3},
        },
        new List<List<double>> {
          new() {7, 8, 2, 3},
          new() {9, 3, 4, 1},
          new() {2, 3, 3, 4},
          new() {5, 1, 2, 6},
        },
        new List<List<double>> {
          new() {58, 35, 29, 44},
          new() {90, 66, 48, 47},
          new() {74, 42, 38, 55},
          new() {70, 52, 27, 39},
        }
      ),
      new(
        new List<List<double>> {
          new() {2, 2, 3, 4, 4, 5},
          new() {4, 5, 6, 1, 2, 3},
          new() {2, 3, 4, 7, 2, 8},
          new() {1, 2, 4, 4, 8, 9},
          new() {6, 8, 8, 9, 2, 1},
          new() {6, 7, 9, 0, 1, 3},
        },
        new List<List<double>> {
          new() {7, 8, 2, 2, 4, 8},
          new() {9, 3, 4, 6, 7, 8},
          new() {2, 3, 3, 5, 4, 1},
          new() {1, 6, 4, 2, 3, 7},
          new() {5, 9, 1, 5, 4, 6},
          new() {4, 2, 8, 3, 4, 1},
        },
        new List<List<double>> {
          new() {82,  101,  81,  74,  82,  92},
          new() {108,  95,  76,  89,  98, 100},
          new() {98,  113, 122,  90, 106, 113},
          new() {113, 140, 118, 109, 114, 113},
          new() {153, 170, 114, 131, 151, 196},
          new() {140, 111,  92, 113, 125, 122},
        }
      )
    };

    return dataList.Select(data => new object[] { data }).ToList();
  }

  [DataTestMethod]
  [DynamicData(nameof(GetDataList), DynamicDataSourceType.Method)]
  public async Task TestMethod1(TestData data) {
    var settings = ServersHelper.ReadServers(pathToServersIp);

    Servers servers = new();
    servers.ConfigureServers(settings);
    
    Client client = new();

    client.SetMatrixA(data.A);
    client.SetMatrixB(data.B);

    BlockMatrix answer = new(data.Result);
    BlockMatrix gettedAns = new(await client.Start(pathToServersIp));

    Assert.AreEqual(answer, gettedAns);
    servers.StopServers();
  }
}
