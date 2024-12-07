namespace ThreadArrExtension;

public static class ThreadArrExtension { 
  public static void StartAll(this Thread[] threads) {
    for (int i = 0; i < threads.Length; ++i) {
      threads[i].Start();
    }
  }

  public static void JoinAll(this Thread[] threads) {
    for (int i = 0; i < threads.Length; ++i) {
      threads[i].Join();
    }
  }
}
