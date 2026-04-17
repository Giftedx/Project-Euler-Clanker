namespace Project_Euler.Problems;

public class Problem076 : Problem {
    public override object Solve() {
        return WaysToSumTo(100);
    }

    // Partition count via coin-change DP using coins {1..n-1}: ways[k] = # of ways to write k as
    // a sum of positive integers < n. Then ways[n] excludes the single-term partition "n" as required.
    private long WaysToSumTo(int n) {
        long[] ways = new long[n + 1];
        ways[0] = 1;
        for (int coin = 1; coin < n; coin++)
            for (int k = coin; k <= n; k++)
                ways[k] += ways[k - coin];
        return ways[n];
    }
}
