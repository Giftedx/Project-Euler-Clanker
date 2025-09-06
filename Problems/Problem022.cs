// ReSharper disable ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator

namespace Project_Euler.Problems;

public class Problem022 : Problem {
    private readonly List<string> _names;

    public Problem022() {
        Library.ReadFile("names.txt", out _names);
    }

    public override object Solve() {
        return SumNameScores();
    }

    private long SumNameScores() {
        _names.Sort();
        long sum = 0;
        for (int i = 0; i < _names.Count; i++) {
            long nameScore = 0;
            foreach (char t in _names[i])
                nameScore += t - 'A' + 1;
            nameScore *= i + 1;
            sum += nameScore;
        }

        return sum;
    }
}