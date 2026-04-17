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

    private static bool IsTriangle(int x) {
        int s = (int)Math.Sqrt(2 * x);
        return s * (s + 1) == 2 * x;
    }
}