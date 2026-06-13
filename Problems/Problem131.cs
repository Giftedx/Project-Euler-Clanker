namespace Project_Euler.Problems;

public class Problem131 : Problem {
    public override object Solve() {
        return CountPrimeCubePartners(1_000_000);
    }

    // n^3 + n^2 * p = k^3. Factor: n^2 (n + p) = k^3.
    // For k = n*m, need (n + p) = n*m^3 / gcd... analysis: p is prime => n = a^3, n + p = b^3 where b > a.
    // So p = b^3 - a^3 = (b - a)(b^2 + ab + a^2). Since p prime, b - a = 1, p = 3a^2 + 3a + 1.
    // Count primes of this form below limit.
    private int CountPrimeCubePartners(int limit) {
        int count = 0;
        for (long a = 1; ; a++) {
            long p = 3 * a * a + 3 * a + 1;
            if (p >= limit) break;
            if (Library.IsPrime((int)p)) count++;
        }
        return count;
    }
}
