namespace Project_Euler.Problems;

public class Problem129 : Problem {
    public override object Solve() {
        return LeastNWithAExceeding(1_000_000);
    }

    // R(k) = (10^k - 1) / 9. A(n) = least k s.t. n divides R(k). Requires gcd(n, 10) = 1.
    // Iterate n from 1_000_000 upward (gcd(n, 10) = 1) and compute A(n). A(n) > limit => return.
    // A(n) <= n always; and is a divisor of phi(9n)/... — but simpler: iterate k with rep remainder.
    private int LeastNWithAExceeding(int limit) {
        for (int n = limit; ; n++) {
            if (n % 2 == 0 || n % 5 == 0) continue;
            long x = 1 % n;
            int k = 1;
            while (x != 0) {
                x = (x * 10 + 1) % n;
                k++;
            }
            if (k > limit) return n;
        }
    }
}
