namespace Project_Euler.Problems;

public class Problem077 : Problem {
    public override object Solve() {
        return FirstOverPrimeSummations(5000);
    }

    // Coin-change DP using primes as coins; scan n=2.. until ways[n] exceeds threshold.
    private int FirstOverPrimeSummations(int threshold) {
        const int upper = 200;
        bool[] sieve = Library.GetSieve(upper);
        long[] ways = new long[upper];
        ways[0] = 1;
        for (int p = 2; p < upper; p++) {
            if (!sieve[p]) continue;
            for (int k = p; k < upper; k++) ways[k] += ways[k - p];
        }
        for (int n = 2; n < upper; n++)
            if (ways[n] > threshold) return n;
        return -1;
    }
}
