using System.Text.Json;

namespace Project_Euler;

public static class BenchmarkConfig {
    public const int DefaultBenchmarkRuns = 100;
    public const int ProgressBarWidth = 50;
    public const int ProgressUpdateIntervalMs = 10;
    public const char ProgressFilledChar = '#';
    public const char ProgressEmptyChar = '-';
    
    public const string LogFile = "log.txt";
    public const string JsonFile = "benchmark.json";
    public const string HtmlFile = "benchmark.html";
    public const string HtmlTemplate = "template.html";
    
    public static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };
    
    public static readonly ParallelOptions DefaultParallelOptions = new() {
        MaxDegreeOfParallelism = Math.Max(1, Environment.ProcessorCount - 1)
    };
}