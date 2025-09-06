using Project_Euler.Interfaces;

namespace Project_Euler.Models;

public class ProblemData(int index, int expectedRuns) {
    public int Index { get; } = index;
    public string Result { get; set; } = "";
    public List<double> Times { get; } = new(expectedRuns);

    private ProblemStatistics? _statistics;

    public ProblemStatistics GetStatistics(IStatisticsCalculator calculator) => 
        _statistics ??= calculator.Calculate(Times);
}