public partial class Program {
  private static async Task Main(string[] args) {
    await Client.Client.Start("../Servers.json");
  }
}
