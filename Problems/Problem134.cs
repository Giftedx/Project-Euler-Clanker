namespace Project_Euler.Problems;

public class Problem134 : Problem {
    public override object Solve() {
        return SumSmallestS(1_000_000);
    }

    // For consecutive primes (p1, p2) with p1 >= 5, find smallest S divisible by p2 whose last digits match p1.
    // S = p1 + k * 10^d, where d = # digits of p1. Need S ≡ 0 mod p2.
    // => k * 10^d ≡ -p1 mod p2 => k ≡ -p1 * inv(10^d) mod p2.
    private long SumSmallestS(int limit) {
        bool[] sieve = Library.GetSieve(limit * 11);
        // Primes p1 from 5 up to first prime > limit (problem says p1 <= 1_000_000)
        long total = 0;
        for (int p1 = 5; p1 <= limit; p1++) {
            if (!sieve[p1]) continue;
            int p2 = p1 + 1;
            while (!sieve[p2]) p2++;

            int d = p1.ToString().Length;
            long pow10 = 1;
            for (int i = 0; i < d; i++) pow10 *= 10;
            long inv = ModInverse(pow10 % p2, p2);
            long k = (((-p1) % p2 + p2) * inv) % p2;
            long S = p1 + k * pow10;
            total += S;
        }
        return total;
    }

    private static long ModInverse(long a, long mod) {
        // Extended Euclidean
        long g = ExtGcd(a, mod, out long x, out _);
        if (g != 1) throw new Exception("no inverse");
        return ((x % mod) + mod) % mod;
    }

    private static long ExtGcd(long a, long b, out long x, out long y) {
        if (b == 0) { x = 1; y = 0; return a; }
        long g = ExtGcd(b, a % b, out long x1, out long y1);
        x = y1;
        y = x1 - (a / b) * y1;
        return g;
    }
}
