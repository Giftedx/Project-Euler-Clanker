namespace Project_Euler.Problems;

public class Problem082 : Problem {
    private readonly int[,] _grid;
    private readonly int _size;

    public Problem082() {
        _grid = Problem081.LoadMatrix("p082_matrix.txt", out _size);
    }

    public override object Solve() {
        return MinPathThreeWay();
    }

    // Process columns left to right. For each column, cost[i] = min over starting row k of
    // sum of column cells from k to i plus previous-column cost[k]. Two sweeps (down, up) give optimum per column.
    private int MinPathThreeWay() {
        int[] cost = new int[_size];
        for (int i = 0; i < _size; i++) cost[i] = _grid[i, 0];

        for (int c = 1; c < _size; c++) {
            int[] next = new int[_size];
            for (int i = 0; i < _size; i++) next[i] = cost[i] + _grid[i, c];

            for (int i = 1; i < _size; i++)
                next[i] = Math.Min(next[i], next[i - 1] + _grid[i, c]);
            for (int i = _size - 2; i >= 0; i--)
                next[i] = Math.Min(next[i], next[i + 1] + _grid[i, c]);

            cost = next;
        }

        int best = int.MaxValue;
        for (int i = 0; i < _size; i++) best = Math.Min(best, cost[i]);
        return best;
    }
}
