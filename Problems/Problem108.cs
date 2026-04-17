namespace Project_Euler.Problems;

public class Problem108 : Problem {
    public override object Solve() {
        return SmallestNWithOver(1000);
    }

    // 1/x + 1/y = 1/n with x >= y > n: each solution corresponds to a divisor pair of n^2.
    // # solutions = (tau(n^2) + 1) / 2 where tau = # divisors. Find min n with this > 1000.
    private int SmallestNWithOver(int threshold) {
        for (int n = 2; ; n++) {
            if ((TauSquared(n) + 1) / 2 > threshold) return n;
        }
    }

    private static int TauSquared(int n) {
        int count = 1;
        for (int p = 2; p * p <= n; p++) {
            if (n % p != 0) continue;
            int e = 0;
            while (n % p == 0) { n /= p; e++; }
            count *= 2 * e + 1;
        }
        if (n > 1) count *= 3;
        return count;
    }
}
