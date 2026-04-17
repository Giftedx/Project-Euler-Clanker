namespace Project_Euler.Problems;

public class Problem128 : Problem {
    public override object Solve() {
        return FindNthPD3Tile(2000);
    }

    // PD=3 tiles:
    //   tile 1 (center)
    //   tile 2 (ring 1 top, special case)
    //   for each ring r >= 2:
    //     start tile T(r) = 3r^2-3r+2 if {6r-1, 6r+1, 12r+5} all prime
    //     end tile T(r)+6r-1 if {6r-1, 6r+5, 12r-7} all prime
    //
    // End-tile primes are 6r-1 (wrap to top), 12r-7 (inner ring upper corner), 6r+5 (outer ring end-wrap).
    // Common confusion: 6r+1 appears in start but not end; 6r+5 appears in end but not start.
    private long FindNthPD3Tile(int N) {
        int found = 0;
        if (++found == N) return 1;
        if (++found == N) return 2;

        for (long r = 2; ; r++) {
            if (IsPrime(6 * r - 1) && IsPrime(6 * r + 1) && IsPrime(12 * r + 5)) {
                long t = 3 * r * r - 3 * r + 2;
                if (++found == N) return t;
            }
            if (IsPrime(6 * r - 1) && IsPrime(6 * r + 5) && IsPrime(12 * r - 7)) {
                long t = 3 * r * r - 3 * r + 2 + 6 * r - 1;
                if (++found == N) return t;
            }
        }
    }

    private static bool IsPrime(long n) {
        if (n < 2) return false;
        if (n < 4) return true;
        if ((n & 1) == 0) return false;
        if (n % 3 == 0) return n == 3;
        long limit = (long)Math.Sqrt(n);
        for (long i = 5; i <= limit; i += 6) {
            if (n % i == 0 || n % (i + 2) == 0) return false;
        }
        return true;
    }
}
