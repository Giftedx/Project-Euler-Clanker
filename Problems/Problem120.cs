namespace Project_Euler.Problems;

public class Problem120 : Problem {
    public override object Solve() {
        return SumMaxRemainders(3, 1000);
    }

    // r_max = max_n ((a-1)^n + (a+1)^n) mod a^2.
    // Binomial expansion: all terms except linear vanish mod a^2, so r = 2*n*a^{n-1} or 2 + 2*n*a (approx).
    // Closed form: if a is even, r_max = a^2 - 2*a; if a is odd, r_max = a^2 - 2*a.
    // Actually for all a >= 3: r_max = a*(a-2) if a even else a*(a-2). Try both and take max over n is easier.
    private long SumMaxRemainders(int aMin, int aMax) {
        long total = 0;
        for (int a = aMin; a <= aMax; a++) {
            long asq = (long)a * a;
            long best = 0;
            long modAsq = asq;
            // (a-1)^n + (a+1)^n mod a^2 simplifies to: 2 * n * a mod a^2 (for n) possibly plus 2 if n even.
            // Specifically: (a-1)^n + (a+1)^n = 2 sum_{k even} C(n,k) a^k -> mod a^2, terms k=0 (->2) and k=1 for odd n (cancel).
            // Actually: (a+1)^n = sum C(n,k) a^k; mod a^2 = 1 + n*a.
            // (a-1)^n mod a^2 = ? For odd n: -(1 + n*a) mod a^2 = a^2 - 1 - n*a. Sum = a^2 + a*n mod a^2 = n*a mod a^2.
            // For even n: 1 + n*a mod a^2 (with sign). (a-1)^n = sum C(n,k)(-1)^{n-k}a^k. n even => k=0: 1; k=1: -n*a. So (a-1)^n mod a^2 = 1 - n*a mod a^2.
            // Sum: (1 + n*a) + (1 - n*a) = 2 mod a^2.
            // So r(n) = (2*n*a) mod a^2 for odd n, 2 for even n.
            // Max r = largest multiple of 2a less than a^2. Take largest odd n such that 2na < a^2, i.e., n < a/2.
            // For a even: largest n=a/2 - 1 odd or n = a/2 - 1 depending; r = 2n*a.
            // For a odd: n = (a-1)/2 gives r = (a-1)*a.
            if (a == 1 || a == 2) { best = 0; total += best; continue; }
            for (int n = 1; n < 2 * a; n++) {
                long r;
                if ((n & 1) == 1) r = (2L * n * a) % asq;
                else r = 2 % asq;
                if (r > best) best = r;
            }
            total += best;
        }
        return total;
    }
}
