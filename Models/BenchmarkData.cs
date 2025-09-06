namespace Project_Euler.Models;

public class BenchmarkData {
    public int SlowestProblem { get; set; }
    public double SlowestTime { get; set; } = double.MinValue;
    public double TotalTime { get; set; }
    public double FastestTime { get; set; } = double.MaxValue;
    public int FastestProblem { get; set; }
    public DateTime Timestamp { get; } = DateTime.Now;
    public string SystemInfo { get; } = GetSystemInfo();
    
    private static string GetSystemInfo() =>
        $"CPU Cores: {Environment.ProcessorCount}, " +
        $"OS: {Environment.OSVersion}, " +
        $".NET: {Environment.Version}";
}