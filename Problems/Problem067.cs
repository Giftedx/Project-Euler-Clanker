namespace Project_Euler.Problems;

public class Problem067 : Problem {
    private readonly int[][] _triangle;

    public Problem067() {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "p067_triangle.txt");
        string[] lines = File.ReadAllLines(path);
        _triangle = new int[lines.Length][];
        for (int i = 0; i < lines.Length; i++) {
            string[] parts = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            _triangle[i] = new int[parts.Length];
            for (int j = 0; j < parts.Length; j++)
                _triangle[i][j] = int.Parse(parts[j]);
        }
    }

    public override object Solve() {
        return MaxPathSum();
    }

    private int MaxPathSum() {
        // Bottom-up: replace each cell with its best downward-path sum.
        int[] row = (int[])_triangle[^1].Clone();
        for (int i = _triangle.Length - 2; i >= 0; i--) {
            int[] above = _triangle[i];
            for (int j = 0; j < above.Length; j++)
                row[j] = above[j] + Math.Max(row[j], row[j + 1]);
        }
        return row[0];
    }
}
