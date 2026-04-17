namespace Project_Euler.Problems;

public class Problem114 : Problem {
    public override object Solve() {
        return CountFillings(50, 3);
    }

    // f(n) = ways to fill row of length n with grey or red-blocks of length >= minBlock, with >= 1 empty cell between red blocks.
    // Recurrence: f(n) = f(n-1)           (grey at position n)
    //                 + sum_{k=minBlock..n} f(n-k-1)   (red block length k ending at pos n, with gap after — last term treats n-k-1 = -1 as base 1)
    private long CountFillings(int n, int minBlock) {
        long[] f = new long[n + 1];
        f[0] = 1;
        for (int i = 1; i <= n; i++) {
            f[i] = f[i - 1]; // grey at i
            for (int k = minBlock; k <= i; k++) {
                // red block of length k ending at position i, with required gap before => uses indices i-k+1..i
                // before block: positions 1..i-k must be a valid fill of length i-k, but the cell immediately before block (i-k) cannot be red's tail.
                // Using standard treatment: after placing block at end, remainder is f(i-k-1) when i-k-1 >= 0, else 1.
                f[i] += i - k - 1 >= 0 ? f[i - k - 1] : 1;
            }
        }
        return f[n];
    }
}
