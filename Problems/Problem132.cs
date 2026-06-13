namespace Project_Euler.Problems;

public class Problem132 : Problem {
    public override object Solve() {
        return SumFirst40PrimeFactorsOfRepunit(1_000_000_000);
    }

    // Prime p divides R(n) iff the order of 10 mod p divides n. For p = 2 or 5, never divides R(n).
    // For odd p with gcd(p, 10) = 1, ord_p(10) | 9n (for R(n) = (10^n-1)/9).
    // Iterate small primes; include if 10^n ≡ 1 (mod p).
    private long SumFirst40PrimeFactorsOfRepunit(int n) {
        long sum = 0;
        int found = 0;
        for (int p = 3; found < 40; p++) {
            if (!Library.IsPrime(p)) continue;
            if (p == 5) continue;
            if (ModPow(10, n, p) == 1) { sum += p; found++; }
        }
        return sum;
    }

    private static long ModPow(long baseVal, long exp, long mod) {
        long result = 1;
        baseVal %= mod;
        while (exp > 0) {
            if ((exp & 1) == 1) result = result * baseVal % mod;
            exp >>= 1;
            baseVal = baseVal * baseVal % mod;
        }
        return result;
    }
}
