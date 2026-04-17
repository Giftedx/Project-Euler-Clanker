namespace Project_Euler.Problems;

public class Problem083 : Problem {
    private readonly int[,] _grid;
    private readonly int _size;

    public Problem083() {
        _grid = Problem081.LoadMatrix("p083_matrix.txt", out _size);
    }

    public override object Solve() {
        return MinPathFourWay();
    }

    // Dijkstra on a 4-connected grid with edge weights = destination-cell value.
    private int MinPathFourWay() {
        int n = _size;
        int[] dist = new int[n * n];
        for (int i = 0; i < dist.Length; i++) dist[i] = int.MaxValue;
        dist[0] = _grid[0, 0];

        var pq = new PriorityQueue<int, int>();
        pq.Enqueue(0, dist[0]);
        int[] dr = { -1, 1, 0, 0 };
        int[] dc = { 0, 0, -1, 1 };

        while (pq.TryDequeue(out int node, out int d)) {
            if (d > dist[node]) continue;
            int r = node / n, c = node % n;
            if (r == n - 1 && c == n - 1) return d;

            for (int k = 0; k < 4; k++) {
                int nr = r + dr[k], nc = c + dc[k];
                if (nr < 0 || nr >= n || nc < 0 || nc >= n) continue;
                int nextIdx = nr * n + nc;
                int nd = d + _grid[nr, nc];
                if (nd >= dist[nextIdx]) continue;
                dist[nextIdx] = nd;
                pq.Enqueue(nextIdx, nd);
            }
        }
        return dist[n * n - 1];
    }
}
