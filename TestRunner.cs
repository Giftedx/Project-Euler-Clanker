using System.Diagnostics;

namespace Project_Euler;

public static class TestRunner {
    private static readonly Dictionary<int, string> ExpectedAnswers = new() {
        {1, "233168"}, {2, "4613732"}, {3, "6857"}, {4, "906609"}, {5, "232792560"},
        {6, "25164150"}, {7, "104743"}, {8, "23514624000"}, {9, "31875000"}, {10, "142913828922"},
        {11, "70600674"}, {12, "76576500"}, {13, "5537376230"}, {14, "837799"}, {15, "137846528820"},
        {16, "1366"}, {17, "21124"}, {18, "8701"}, {19, "171"}, {20, "648"},
        {21, "31626"}, {22, "871198282"}, {23, "4179871"}, {24, "2783915460"}, {25, "4782"},
        {26, "983"}, {27, "-59231"}, {28, "669171001"}, {29, "9183"}, {30, "443839"},
        {31, "73682"}, {32, "45228"}, {33, "100"}, {34, "40730"}, {35, "55"},
        {36, "872187"}, {37, "748317"}, {38, "932718654"}, {39, "840"}, {40, "210"},
        {41, "7652413"}, {42, "162"}, {43, "16695334890"}, {44, "5482660"}, {45, "1533776805"},
        {46, "5777"}, {47, "134043"}, {48, "9110846700"}, {49, "296962999629"}, {50, "997651"},
        {51, "121313"}, {52, "142857"}, {53, "4075"}, {54, "376"}, {55, "249"},
        {56, "972"}, {57, "153"}, {58, "26241"}, {59, "129448"}, {60, "26033"},
        {61, "28684"}, {62, "127035954683"}, {63, "49"}, {64, "1322"}, {65, "272"},
        {66, "661"}, {67, "7273"}, {68, "6531031914842725"}, {69, "510510"}, {70, "8319823"}
    };

    public static void RunAll() {
        Library.PrecomputePrimes(100000);

        int total = ProblemFactory.SolvedProblems();
        Console.WriteLine($"Running {total} problems...\n");

        int passed = 0, failed = 0, errors = 0;
        var overallWatch = Stopwatch.StartNew();

        for (int i = 1; i <= total; i++) {
            try {
                var problem = ProblemFactory.CreateProblem(i);
                problem.Solve(); // warm up

                var watch = Stopwatch.StartNew();
                object result = problem.Solve();
                watch.Stop();

                string resultStr = result.ToString() ?? "";
                string status;

                if (ExpectedAnswers.TryGetValue(i, out string? expected)) {
                    if (resultStr == expected) {
                        status = "PASS";
                        passed++;
                    } else {
                        status = $"FAIL (expected {expected})";
                        failed++;
                    }
                } else {
                    status = "NO ANSWER KEY";
                    passed++; // don't count as failure
                }

                Console.WriteLine($"Problem {i:D3}: {resultStr,-20} {status,-30} ({watch.Elapsed.TotalMilliseconds:F3} ms)");
            } catch (Exception ex) {
                Console.WriteLine($"Problem {i:D3}: ERROR - {ex.Message}");
                errors++;
            }
        }

        overallWatch.Stop();
        Console.WriteLine($"\n{passed} passed, {failed} failed, {errors} errors — {overallWatch.Elapsed.TotalMilliseconds:F0} ms total");

        if (failed > 0 || errors > 0)
            Environment.ExitCode = 1;
    }
}
