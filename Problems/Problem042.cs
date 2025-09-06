// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator

namespace Project_Euler.Problems;

public class Problem042 : Problem {
    private readonly List<string> _words;

    public Problem042() {
        Library.ReadFile("words.txt", out _words);
    }

    public override object Solve() {
        return CountWordScoreTriangleNums();
    }

    private int CountWordScoreTriangleNums() {
        return _words.Count(word => IsTriangle(WordValue(word)));
    }

    private int WordValue(string s) {
        int sum = 0;
        foreach (char c in s)
            sum += c - 'A' + 1;
        return sum;
    }

    private bool IsTriangle(int x) {
        double n = (-1 + Math.Sqrt(1 + 8 * x)) / 2;
        return n == (int)n;
    }
}