using Project_Euler.Interfaces;
using Project_Euler.Models;

namespace Project_Euler.Services;

public class StatisticsCalculator : IStatisticsCalculator {
    public ProblemStatistics Calculate(List<double> times) {
        if (times.Count == 0) 
            return new ProblemStatistics();
        
        double[] sortedTimes = times.OrderBy(t => t).ToArray();
        double average = times.Average();
        
        return new ProblemStatistics {
            Best = sortedTimes[0],
            Worst = sortedTimes[^1],
            Average = average,
            Median = CalculateMedian(sortedTimes),
            StandardDeviation = CalculateStandardDeviation(times, average)
        };
    }
    
    public BenchmarkData CalculateBenchmarkData(List<ProblemData> results) {
        var benchmarkData = new BenchmarkData();
        
        foreach (var result in results) {
            var stats = result.GetStatistics(this);
            benchmarkData.TotalBestTime += stats.Best;
            
            if (stats.Worst > benchmarkData.SlowestTime) {
                benchmarkData.SlowestTime = stats.Worst;
                benchmarkData.SlowestProblem = result.Index;
            }

            if (!(stats.Best < benchmarkData.FastestTime)) continue;
            benchmarkData.FastestTime = stats.Best;
            benchmarkData.FastestProblem = result.Index;
        }
        
        return benchmarkData;
    }
    
    private static double CalculateMedian(double[] sortedTimes) {
        int count = sortedTimes.Length;
        if (count % 2 == 0) return (sortedTimes[count / 2 - 1] + sortedTimes[count / 2]) / 2.0;
        return sortedTimes[count / 2];
    }
    
    private static double CalculateStandardDeviation(List<double> times, double average) {
        if (times.Count <= 1) return 0;
        
        double sumSquaredDeviations = times.Sum(time => Math.Pow(time - average, 2));
        return Math.Sqrt(sumSquaredDeviations / (times.Count - 1));
    }
}