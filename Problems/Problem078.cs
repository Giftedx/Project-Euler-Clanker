namespace Project_Euler.Problems;

public class Problem078 : Problem {
    public override object Solve() {
        return LeastNDivPartition(1_000_000);
    }

    // Euler's pentagonal number theorem: p(n) = sum_k (-1)^(k-1) [p(n - g_k) + p(n - g_{k+1})]
    // where g_k = k(3k-1)/2 are generalized pentagonal numbers. Compute mod divisor.
    private int LeastNDivPartition(int divisor) {
        var p = new List<int> { 1 };
        for (int n = 1; ; n++) {
            long total = 0;
            for (int k = 1; ; k++) {
                int g1 = k * (3 * k - 1) / 2;
                int g2 = k * (3 * k + 1) / 2;
                if (g1 > n) break;
                int sign = (k & 1) == 1 ? 1 : -1;
                total += sign * p[n - g1];
                if (g2 <= n) total += sign * p[n - g2];
            }
            total %= divisor;
            if (total < 0) total += divisor;
            if (total == 0) return n;
            p.Add((int)total);
        }
    }
}
