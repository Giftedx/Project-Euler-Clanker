namespace Project_Euler.Problems;

public class Problem133 : Problem {
    public override object Solve() {
        return SumPrimesNotDividingAnyPower10Repunit(100_000);
    }

    // p divides R(10^k) for some k iff ord_p(10) has only 2 and 5 as prime factors.
    // So sum primes p < limit where ord_p(10) has a prime factor other than 2, 5 — these "never divide".
    private long SumPrimesNotDividingAnyPower10Repunit(int limit) {
        bool[] sieve = Library.GetSieve(limit);
        long sum = 2 + 5; // p = 2 and p = 5 never divide repunit
        for (int p = 3; p < limit; p++) {
            if (!sieve[p]) continue;
            if (p == 5) continue;
            int order = OrderOf10(p);
            // strip 2s and 5s
            while (order % 2 == 0) order /= 2;
            while (order % 5 == 0) order /= 5;
            if (order != 1) sum += p;
        }
        return sum;
    }

    private static int OrderOf10(int p) {
        int phi = p - 1; // p prime => phi(p) = p-1
        // order divides phi
        int order = phi;
        for (int d = 2; d * d <= phi; d++) {
            if (phi % d != 0) continue;
            if (ModPow(10, d, p) == 1 && d < order) order = d;
            int other = phi / d;
            if (ModPow(10, other, p) == 1 && other < order) order = other;
        }
        // Refine: try all divisors (via prime factorization of phi)
        int n = phi;
        var factors = new List<int>();
        for (int i = 2; i * i <= n; i++) {
            while (n % i == 0) { factors.Add(i); n /= i; }
        }
        if (n > 1) factors.Add(n);
        int ord = phi;
        foreach (int f in factors) {
            while (ord % f == 0 && ModPow(10, ord / f, p) == 1) ord /= f;
        }
        return ord;
    }

    private static int ModPow(long baseVal, long exp, long mod) {
        long result = 1;
        baseVal %= mod;
        while (exp > 0) {
            if ((exp & 1) == 1) result = result * baseVal % mod;
            exp >>= 1;
            baseVal = baseVal * baseVal % mod;
        }
        return (int)result;
    }
}
