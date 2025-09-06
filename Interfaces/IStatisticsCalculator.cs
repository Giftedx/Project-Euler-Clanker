using Project_Euler.Models;

namespace Project_Euler.Interfaces;

public interface IStatisticsCalculator {
    ProblemStatistics Calculate(List<double> times);
    BenchmarkData CalculateBenchmarkData(List<ProblemData> results);
}