namespace Project_Euler.Problems;

public class Problem060 : Problem {
    private int[] _primes = null!;
    private bool[] _sieve = null!;
    private readonly Dictionary<long, bool> _cache = new();

    public override object Solve() => FindPrimePairSet();

    private int FindPrimePairSet() {
        const int limit = 10000;
        _sieve = Library.GetSieve(limit);

        var primeList = new List<int>();
        for (int i = 2; i < limit; i++) {
            if (_sieve[i]) primeList.Add(i);
        }
        _primes = primeList.ToArray();
        int n = _primes.Length;

        // Build adjacency: adj[i] = sorted list of indices j > i where pair(i,j) works
        var adj = new List<int>[n];
        for (int i = 0; i < n; i++) adj[i] = new List<int>();

        for (int i = 0; i < n; i++) {
            for (int j = i + 1; j < n; j++) {
                if (AreConcatPrime(_primes[i], _primes[j])) {
                    adj[i].Add(j);
                }
            }
        }

        int best = int.MaxValue;

        for (int a = 0; a < n && _primes[a] * 5 < best; a++) {
            var aList = adj[a];

            for (int bi = 0; bi < aList.Count; bi++) {
                int b = aList[bi];
                if (_primes[a] + _primes[b] * 4 >= best) break;
                var bList = adj[b];

                for (int ci = bi + 1; ci < aList.Count; ci++) {
                    int c = aList[ci];
                    if (_primes[a] + _primes[b] + _primes[c] * 3 >= best) break;
                    if (!bList.Contains(c)) continue;
                    var cList = adj[c];

                    for (int di = ci + 1; di < aList.Count; di++) {
                        int d = aList[di];
                        if (_primes[a] + _primes[b] + _primes[c] + _primes[d] * 2 >= best) break;
                        if (!bList.Contains(d) || !cList.Contains(d)) continue;
                        var dList = adj[d];

                        for (int ei = di + 1; ei < aList.Count; ei++) {
                            int e = aList[ei];
                            int sum = _primes[a] + _primes[b] + _primes[c] + _primes[d] + _primes[e];
                            if (sum >= best) break;
                            if (bList.Contains(e) && cList.Contains(e) && dList.Contains(e)) {
                                best = sum;
                            }
                        }
                    }
                }
            }
        }

        return best == int.MaxValue ? -1 : best;
    }

    private bool AreConcatPrime(int a, int b) {
        return IsPrimeFast(Concat(a, b)) && IsPrimeFast(Concat(b, a));
    }

    private static long Concat(int a, int b) {
        long mul = 10;
        while (mul <= b) mul *= 10;
        return a * mul + b;
    }

    private bool IsPrimeFast(long n) {
        if (n < _sieve.Length) return _sieve[n];
        if (_cache.TryGetValue(n, out bool cached)) return cached;

        bool result = MillerRabin(n);
        _cache[n] = result;
        return result;
    }

    private static bool MillerRabin(long n) {
        if (n < 2) return false;
        if (n is 2 or 3 or 5 or 7) return true;
        if (n % 2 == 0 || n % 3 == 0 || n % 5 == 0) return false;

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
