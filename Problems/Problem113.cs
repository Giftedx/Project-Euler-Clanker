namespace Project_Euler.Problems;

public class Problem113 : Problem {
    public override object Solve() {
        return CountNonBouncyBelow(100);
    }

    // Non-decreasing n-digit numbers = C(n+9, 9) - 1 (subtract all zeros).
    // Non-increasing n-digit numbers = C(n+9, 9) - n - 1 (subtract sequences starting with 0).
    // Double-counted: repdigits = 9 * n.
    // Total non-bouncy = non_dec + non_inc - repdigits — but careful with constant digit overlap.
    private long CountNonBouncyBelow(int digits) {
        // Non-decreasing of up to n digits (allow leading zeros but subtract "000..0"):
        long nonDec = Binomial(digits + 9, 9) - 1;
        // Non-increasing: stars-and-bars over digits 0..9 giving length n with leading zeros excluded.
        long nonInc = Binomial(digits + 10, 10) - 1 - digits; // subtract all-zero and each length's leading-zero-but-nonzero cases
        // Repdigits counted in both (except constant 0): 9 per digit length * n digit lengths
        long rep = 9L * digits;
        return nonDec + nonInc - rep;
    }

    private static long Binomial(int n, int k) {
        if (k < 0 || k > n) return 0;
        if (k > n - k) k = n - k;
        long r = 1;
        for (int i = 0; i < k; i++) r = r * (n - i) / (i + 1);
        return r;
    }
}
