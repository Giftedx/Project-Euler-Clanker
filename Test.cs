using System.Collections.Concurrent;
using static Project_Euler.Library;

namespace Project_Euler;

public class Test {
    private readonly List<int> _primes;
    private readonly ConcurrentDictionary<long, bool> _primeCache = new();

    public Test() {
        GetPrimeList(10000, out _primes);
    }
    
    public void Solve() {
        Console.WriteLine(FindMinimalPrimeCliqueSum());
    }

    private int FindMinimalPrimeCliqueSum() {
        var graph = new Dictionary<int, List<int>>();

        for (int i = 0; i < _primes.Count; i++) {
            int p = _primes[i];
            for (int j = i + 1; j < _primes.Count; j++) {
                int q = _primes[j];
                if (!AreConcatenationsPrime(p, q)) continue;
                if (!graph.TryGetValue(p, out var list)) graph[p] = list = [];
                list.Add(q);
            }
        }

        var minSums = new ConcurrentBag<int>();

        Parallel.ForEach(_primes, a => {
            if (!graph.TryGetValue(a, out var aList)) return;

            foreach (int b in aList) {
                if (!graph.TryGetValue(b, out var bList)) continue;

                foreach (int c in aList.Where(c => c > b && bList.Contains(c))) {
                    if (!graph.TryGetValue(c, out var cList)) continue;

                    foreach (int d in aList.Where(d => d > c && bList.Contains(d) && cList.Contains(d))) {
                        if (!graph.TryGetValue(d, out var dList)) continue;

                        foreach (int sum in from e in aList 
                                 where e > d && bList.Contains(e) && cList.Contains(e) 
                                       && dList.Contains(e) select a + b + c + d + e) {
                            minSums.Add(sum);
                        }
                    }
                }
            }
        });

        return minSums.Count > 0 ? minSums.Min() : -1;
    }

    private bool AreConcatenationsPrime(int a, int b) {
        return IsPrime(ConcatInts(a, b)) && IsPrime(ConcatInts(b, a));
    }

    private long ConcatInts(int a, int b) {
        long multiplier = 10;
        while (multiplier <= b) multiplier *= 10;
        return a * multiplier + b;
    }

    private bool IsPrime(long n) {
        if (n < 2) return false;
        if (_primeCache.TryGetValue(n, out var cached)) return cached;

        if (n % 2 == 0) return n == 2;
        bool isPrime = MillerRabin(n, 5);
        _primeCache.TryAdd(n, isPrime);
        return isPrime;
    }

    private bool MillerRabin(long n, int k) {
        if (n < 2) return false;
        if (n is 2 or 3) return true;

        long d = n - 1;
        int s = 0;
        while ((d & 1) == 0) {
            d >>= 1;
            s++;
        }

        Random rng = new();
        for (int i = 0; i < k; i++) {
            long a = rng.Next(2, (int)Math.Min(n - 2, int.MaxValue));
            long x = ModPow(a, d, n);
            if (x == 1 || x == n - 1) continue;

            bool passed = false;
            for (int r = 1; r < s; r++) {
                x = ModPow(x, 2, n);
                if (x == n - 1) {
                    passed = true;
                    break;
                }
            }

            if (!passed) return false;
        }
        return true;
    }

    private long ModPow(long baseVal, long exponent, long mod) {
        long result = 1;
        baseVal %= mod;
        while (exponent > 0) {
            if ((exponent & 1) == 1) result = result * baseVal % mod;
            baseVal = baseVal * baseVal % mod;
            exponent >>= 1;
        }
        return result;
    }
}