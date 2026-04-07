using System.Collections.Concurrent;

namespace Project_Euler.Problems;

public class Problem060 : Problem {
    private static List<int>? primes;
    private static bool[]? sieve;
    private static readonly ConcurrentDictionary<long, bool> primeCache = new();
    
    public Problem060() => GeneratePrimes(10000);
    public override object Solve() => FindPrimePairSet();

    private void GeneratePrimes(int limit) {
        sieve = new bool[limit + 1];
        Array.Fill(sieve, true);
        sieve[0] = sieve[1] = false;
        
        for (int i = 2; i * i <= limit; i++) {
            if (!sieve[i]) continue;
            for (int j = i * i; j <= limit; j += i) sieve[j] = false;
        }
        
        primes = [];
        for (int i = 2; i <= limit; i++) {
            if (!sieve[i]) continue;
            primes.Add(i);
            primeCache[i] = true;
        }
    }

    private int FindPrimePairSet() {
        var graph = new Dictionary<int, HashSet<int>>();

        if (primes != null) {
            var chunks = Partitioner.Create(0, primes.Count, Math.Max(1, primes.Count / Environment.ProcessorCount));
            var localGraphs = new ConcurrentBag<Dictionary<int, HashSet<int>>>();
        
            Parallel.ForEach(chunks, chunk => {
                var localGraph = new Dictionary<int, HashSet<int>>();
            
                for (int i = chunk.Item1; i < chunk.Item2; i++) {
                    int p = primes[i];
                    for (int j = i + 1; j < primes.Count; j++) {
                        int q = primes[j];
                        if (!AreConcatenationsPrime(p, q)) continue;
                    
                        if (!localGraph.ContainsKey(p)) localGraph[p] = [];
                        if (!localGraph.ContainsKey(q)) localGraph[q] = [];
                        localGraph[p].Add(q);
                        localGraph[q].Add(p);
                    }
                }
                localGraphs.Add(localGraph);
            });
        
            foreach (var localGraph in localGraphs) {
                foreach (var kvp in localGraph) {
                    if (!graph.ContainsKey(kvp.Key)) graph[kvp.Key] = new HashSet<int>();
                    graph[kvp.Key].UnionWith(kvp.Value);
                }
            }
        }

        if (primes == null) return 0;
        var candidates = new List<int>();
        foreach (int p in primes) {
            if (graph.TryGetValue(p, out var neighbors) && neighbors.Count >= 4)
                candidates.Add(p);
        }

        int minSum = int.MaxValue;

        Parallel.ForEach(candidates, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
            () => int.MaxValue,
            (a, _, localMin) => {
                if (!graph.TryGetValue(a, out var aNeighbors)) return localMin;

                var aList = new List<int>();
                foreach (int x in aNeighbors) {
                    if (x > a) aList.Add(x);
                }
                if (aList.Count < 4) return localMin;

                for (int bi = 0; bi < aList.Count; bi++) {
                    int b = aList[bi];
                    if (!graph.TryGetValue(b, out var bNeighbors)) continue;

                    var commonAB = new List<int>();
                    for (int ci = bi + 1; ci < aList.Count; ci++) {
                        if (bNeighbors.Contains(aList[ci]))
                            commonAB.Add(aList[ci]);
                    }
                    if (commonAB.Count < 3) continue;

                    for (int ci = 0; ci < commonAB.Count; ci++) {
                        int c = commonAB[ci];
                        if (!graph.TryGetValue(c, out var cNeighbors)) continue;

                        var commonABC = new List<int>();
                        for (int di = ci + 1; di < commonAB.Count; di++) {
                            int d = commonAB[di];
                            if (bNeighbors.Contains(d) && cNeighbors.Contains(d))
                                commonABC.Add(d);
                        }
                        if (commonABC.Count < 2) continue;

                        for (int di = 0; di < commonABC.Count; di++) {
                            int d = commonABC[di];
                            if (!graph.TryGetValue(d, out var dNeighbors)) continue;

                            for (int ei = di + 1; ei < commonABC.Count; ei++) {
                                int e = commonABC[ei];
                                if (bNeighbors.Contains(e) && cNeighbors.Contains(e) && dNeighbors.Contains(e)) {
                                    int sum = a + b + c + d + e;
                                    if (sum < localMin) localMin = sum;
                                }
                            }
                        }
                    }
                }

                return localMin;
            },
            localMin => {
                if (localMin < int.MaxValue) {
                    int current = Volatile.Read(ref minSum);
                    while (localMin < current) {
                        int prev = Interlocked.CompareExchange(ref minSum, localMin, current);
                        if (prev == current) break;
                        current = prev;
                    }
                }
            });
        
        return minSum == int.MaxValue ? -1 : minSum;
    }

    private bool AreConcatenationsPrime(int a, int b) => IsPrime(ConcatInts(a, b)) && IsPrime(ConcatInts(b, a));

    private static long ConcatInts(int a, int b) {
        long multiplier = 10;
        while (multiplier <= b) multiplier *= 10;
        return a * multiplier + b;
    }

    private bool IsPrime(long n) {
        if (sieve != null && n < sieve.Length) return sieve[n];
        if (primeCache.TryGetValue(n, out bool cached)) return cached;
        
        bool isPrime = IsPrimeFast(n);
        primeCache.TryAdd(n, isPrime);
        return isPrime;
    }

    private bool IsPrimeFast(long n) {
        switch (n) {
            case < 2:
                return false;
            case 2:
            case 3:
                return true;
        }

        if (n % 2 == 0 || n % 3 == 0) return false;

        if (primes != null)
            for (int i = 2; i < primes.Count && (long)primes[i] * primes[i] <= n; i++)
                if (n % primes[i] == 0)
                    return false;

        long sqrt_n = (long)Math.Sqrt(n);
        if (primes == null) return true;
        for (long i = primes.Count > 2 ? primes[^1] : 5; i <= sqrt_n; i += 6)
            if (n % i == 0 || n % (i + 2) == 0)
                return false;

        return true;
    }
}