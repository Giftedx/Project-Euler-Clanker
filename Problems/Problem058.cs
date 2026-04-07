namespace Project_Euler.Problems;

public class Problem058 : Problem {
    private readonly Dictionary<long, bool> _cache = new();

    public override object Solve() {
        return SpiralPrimes();
    }

    private int SpiralPrimes() {
        int primeCount = 0;
        int totalDiagonals = 1;
        int sideLength = 1;

        while (sideLength == 1 || (double)primeCount / totalDiagonals >= 0.10) {
            sideLength += 2;
            long square = (long)sideLength * sideLength;
            int step = sideLength - 1;

            for (int i = 1; i <= 3; i++) {
                long cornerValue = square - (long)i * step;
                if (IsPrimeCached(cornerValue)) primeCount++;
            }

            totalDiagonals += 4;
        }

        return sideLength;
    }

    private bool IsPrimeCached(long n) {
        if (_cache.TryGetValue(n, out bool cached)) return cached;
        bool result = MillerRabin(n);
        _cache[n] = result;
        return result;
    }

    private static bool MillerRabin(long n) {
        if (n < 2) return false;
        if (n is 2 or 3 or 5 or 7) return true;
        if (n % 2 == 0 || n % 3 == 0 || n % 5 == 0) return false;
        if (n % 7 == 0 || n % 11 == 0 || n % 13 == 0 || n % 17 == 0 ||
            n % 19 == 0 || n % 23 == 0 || n % 29 == 0 || n % 31 == 0)
            return n is 7 or 11 or 13 or 17 or 19 or 23 or 29 or 31;

        long d = n - 1;
        int r = 0;
        while ((d & 1) == 0) { d >>= 1; r++; }

        ReadOnlySpan<long> witnesses = [2, 3, 5, 7];
        foreach (long a in witnesses) {
            if (a >= n) continue;
            long x = ModPow(a, d, n);
            if (x == 1 || x == n - 1) continue;

            bool composite = true;
            for (int i = 0; i < r - 1; i++) {
                x = x * x % n;
                if (x == n - 1) { composite = false; break; }
            }
            if (composite) return false;
        }
        return true;
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
