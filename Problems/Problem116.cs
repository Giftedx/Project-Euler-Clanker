namespace Project_Euler.Problems;

public class Problem116 : Problem {
    public override object Solve() {
        return TotalWaysSingleColor(50);
    }

    // For each block size m in {2,3,4}: ways to fill n-cell row with zero or more m-blocks (not adjacent not required) and grey.
    // Excluding the empty arrangement: f(n, m) - 1, where f(n, m) = f(n-1, m) + f(n-m, m).
    private long TotalWaysSingleColor(int n) {
        long total = 0;
        foreach (int m in new[] { 2, 3, 4 }) {
            long[] f = new long[n + 1];
            f[0] = 1;
            for (int i = 1; i <= n; i++) {
                f[i] = f[i - 1];
                if (i >= m) f[i] += f[i - m];
            }
            total += f[n] - 1;
        }
        return total;
    }
}
