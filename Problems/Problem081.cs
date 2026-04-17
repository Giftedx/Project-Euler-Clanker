namespace Project_Euler.Problems;

public class Problem081 : Problem {
    private readonly int[,] _grid;
    private readonly int _size;

    public Problem081() {
        _grid = LoadMatrix("p081_matrix.txt", out _size);
    }

    public override object Solve() {
        return MinPathTwoWay();
    }

    // DP: each cell = cell + min(up, left). Only right and down moves allowed, reversed perspective.
    private int MinPathTwoWay() {
        int[,] dp = new int[_size, _size];
        dp[0, 0] = _grid[0, 0];
        for (int i = 1; i < _size; i++) dp[i, 0] = dp[i - 1, 0] + _grid[i, 0];
        for (int j = 1; j < _size; j++) dp[0, j] = dp[0, j - 1] + _grid[0, j];
        for (int i = 1; i < _size; i++)
            for (int j = 1; j < _size; j++)
                dp[i, j] = _grid[i, j] + Math.Min(dp[i - 1, j], dp[i, j - 1]);
        return dp[_size - 1, _size - 1];
    }

    internal static int[,] LoadMatrix(string fileName, out int size) {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", fileName);
        string[] lines = File.ReadAllLines(path);
        size = lines.Length;
        int[,] matrix = new int[size, size];
        for (int i = 0; i < size; i++) {
            string[] parts = lines[i].Split(',');
            for (int j = 0; j < size; j++)
                matrix[i, j] = int.Parse(parts[j]);
        }
        return matrix;
    }
}
