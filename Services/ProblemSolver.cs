using System.Collections.Concurrent;
using System.Diagnostics;
using Project_Euler.Interfaces;
using Project_Euler.Models;

namespace Project_Euler.Services;

public class ProblemSolver(IOutputHandler outputHandler, IStatisticsCalculator statisticsCalculator)
    : IProblemSolver {
    public void IndividualBenchmark(string problem) {
        int n = Convert.ToInt32(problem);
        var data = Run(n, 1);
        var stats = data.GetStatistics(statisticsCalculator);
        
        Library.FunPrint($"Problem {n}: {data.Result}");
        Library.FunPrint($"Solved in {stats.Best:F3} ms");
    }
    
    public void FullBenchmark() {
        (int start, int end) = InputHandler.GetBenchmarkRange();
        RunBenchmarkRange(start, end);
    }

    private void RunBenchmarkRange(int start, int end) {
        var bag = new ConcurrentBag<ProblemData>();
        var range = Enumerable.Range(start, end - start + 1);
        int problemCount = end - start + 1;
    
        int completedTasks = 0;
        using var cancellationTokenSource = new CancellationTokenSource();
    
        var completedTasksFunc = () => Volatile.Read(ref completedTasks);
        var cancellationToken = cancellationTokenSource.Token;

        var progressThread = new Thread(() => 
            DisplayProgressBar(completedTasksFunc, problemCount, cancellationToken)) {
            IsBackground = true,
            Name = "ProgressReporter"
        };
    
        var watch = Stopwatch.StartNew();
        progressThread.Start();
    
        Parallel.ForEach(range, BenchmarkConfig.DefaultParallelOptions, i => {
            bag.Add(Run(i));
            Interlocked.Increment(ref completedTasks);
        });
    
        Thread.Sleep(100);
    
        cancellationTokenSource.Cancel();
        progressThread.Join(TimeSpan.FromSeconds(1));
    
        var results = bag.OrderBy(p => p.Index).ToList();
        var testData = statisticsCalculator.CalculateBenchmarkData(results);
    
        watch.Stop();
        outputHandler.GenerateFullReport(results, testData);
        Library.FunPrint("");
        Library.FunPrint($"Results output to {BenchmarkConfig.LogFile}, " +
                         $"{watch.ElapsedMilliseconds} ms total");
        Library.FunPrint($"Benchmarked problems {start} to {end} ({problemCount} problems)");
    }

    private static ProblemData Run(int n, int runs = BenchmarkConfig.DefaultBenchmarkRuns) {
        var data = new ProblemData(n, runs);
        var problem = ProblemFactory.CreateProblem(n);

        // Warm-up: let JIT compile hot paths before timing
        for (int w = 0; w < BenchmarkConfig.WarmupRuns; w++)
            problem.Solve();

        // Clean heap state before measurement
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
        GC.WaitForPendingFinalizers();

        for (int i = 0; i < runs; i++) {
            var watch = Stopwatch.StartNew();

            object result = problem.Solve();
            watch.Stop();

            if (i == 0) data.Result = result.ToString() ?? string.Empty;
            data.Times.Add(watch.Elapsed.TotalMilliseconds);
        }

        return data;
    }
    
    public void RunTest() {
        Test test = new Test();
        var watch = Stopwatch.StartNew();
        test.Solve();
        watch.Stop();
        Console.WriteLine($"{watch.ElapsedMilliseconds} ms");
    }
    
    private static void DisplayProgressBar(Func<int> completedFunc, int total, CancellationToken cancellationToken) {
        while (!cancellationToken.IsCancellationRequested) {
            int completed = completedFunc();
            double percent = Math.Min(1.0, (double)completed / total);
            int filled = (int)(percent * BenchmarkConfig.ProgressBarWidth);
            
            string bar = new string(BenchmarkConfig.ProgressFilledChar, filled) +
                        new string(BenchmarkConfig.ProgressEmptyChar, BenchmarkConfig.ProgressBarWidth - filled);
            
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write($"[{bar}] {percent * 100:F1}% ({completed}/{total})");
            
            if (completed >= total) {
                Console.WriteLine();
                break;
            }
            
            try {
                Thread.Sleep(BenchmarkConfig.ProgressUpdateIntervalMs);
            } catch (ThreadInterruptedException) {
                Console.WriteLine();
                break;
            }
        }
    }
}