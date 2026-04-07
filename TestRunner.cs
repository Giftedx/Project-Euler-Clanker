using System.Diagnostics;

namespace Project_Euler;

public static class TestRunner {
    public static void RunAll() {
        Library.PrecomputePrimes(100000);

        int total = ProblemFactory.SolvedProblems();
        Console.WriteLine($"Running {total} problems...\n");

        var overallWatch = Stopwatch.StartNew();

        for (int i = 1; i <= total; i++) {
            try {
                var problem = ProblemFactory.CreateProblem(i);
                problem.Solve(); // warm up

                var watch = Stopwatch.StartNew();
                object result = problem.Solve();
                watch.Stop();

                Console.WriteLine($"Problem {i:D3}: {result,-20} ({watch.Elapsed.TotalMilliseconds:F3} ms)");
            } catch (Exception ex) {
                Console.WriteLine($"Problem {i:D3}: ERROR - {ex.Message}");
            }
        }

        overallWatch.Stop();
        Console.WriteLine($"\nTotal: {overallWatch.Elapsed.TotalMilliseconds:F0} ms");
    }
}
