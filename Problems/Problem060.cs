namespace Project_Euler.Problems;

public class Problem060 : Problem {
    private int[] _primes = null!;
    private List<int>[] _adj = null!;
    private readonly Dictionary<long, bool> _primeCache = new();

    public override object Solve() {
        BuildGraph();
        return FindClique();
    }

    private void BuildGraph() {
        const int limit = 10000;
        bool[] sieve = Library.GetSieve(limit);

        var primeList = new List<int>();
        for (int i = 2; i < limit; i++) {
            if (sieve[i]) primeList.Add(i);
        }
        _primes = primeList.ToArray();
        int n = _primes.Length;

        _adj = new List<int>[n];
        for (int i = 0; i < n; i++) _adj[i] = new List<int>();

        for (int i = 0; i < n; i++) {
            int pi = _primes[i];
            int ri = pi % 3;
            for (int j = i + 1; j < n; j++) {
                int pj = _primes[j];
                // concat(a,b) ≡ a+b (mod 3) — skip if ≡ 0 and neither is 3
                if (ri != 0 && pj % 3 != ri) continue;
                if (AreConcatPrime(pi, pj, sieve)) {
                    _adj[i].Add(j);
                }
            }
        }
    }

    private int FindClique() {
        int n = _primes.Length;
        int best = int.MaxValue;

        for (int a = 0; a < n && _primes[a] * 5 < best; a++) {
            var aList = _adj![a];

            for (int bi = 0; bi < aList.Count; bi++) {
                int b = aList[bi];
                if (_primes[a] + _primes[b] * 4 >= best) break;
                var bList = _adj[b];

                for (int ci = bi + 1; ci < aList.Count; ci++) {
                    int c = aList[ci];
                    if (_primes[a] + _primes[b] + _primes[c] * 3 >= best) break;
                    if (!bList.Contains(c)) continue;
                    var cList = _adj[c];

                    for (int di = ci + 1; di < aList.Count; di++) {
                        int d = aList[di];
                        if (_primes[a] + _primes[b] + _primes[c] + _primes[d] * 2 >= best) break;
                        if (!bList.Contains(d) || !cList.Contains(d)) continue;
                        var dList = _adj[d];

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

    private bool AreConcatPrime(int a, int b, bool[] sieve) {
        return IsPrimeFast(Concat(a, b), sieve) && IsPrimeFast(Concat(b, a), sieve);
    }

    private static long Concat(int a, int b) {
        long mul = 10;
        while (mul <= b) mul *= 10;
        return a * mul + b;
    }

    private bool IsPrimeFast(long n, bool[] sieve) {
        if (n < sieve.Length) return sieve[n];
        if (_primeCache.TryGetValue(n, out bool cached)) return cached;
        bool result = MillerRabin(n);
        _primeCache[n] = result;
        return result;
    }

    private static bool MillerRabin(long n) {
        if (n < 2) return false;
        if (n is 2 or 3 or 5 or 7) return true;
        if (n % 2 == 0 || n % 3 == 0 || n % 5 == 0) return false;
        if (n % 7 == 0 || n % 11 == 0 || n % 13 == 0 || n % 17 == 0 ||
            n % 19 == 0 || n % 23 == 0 || n % 29 == 0 || n % 31 == 0 ||
            n % 37 == 0 || n % 41 == 0 || n % 43 == 0)
            return n is 7 or 11 or 13 or 17 or 19 or 23 or 29 or 31 or 37 or 41 or 43;

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
