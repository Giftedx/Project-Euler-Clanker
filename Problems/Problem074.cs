namespace Project_Euler.Problems;

public class Problem074 : Problem {
    private static readonly int[] Fact = { 1, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880 };

    public override object Solve() {
        return CountChainsOfLength60(1_000_000);
    }

    // For n < 10^6 the digit factorial sum stays within ~2.2M, so an array cache keyed by value works.
    // cache[v]: -1 = unknown; otherwise = chain length from v (# distinct terms before a repeat).
    private int CountChainsOfLength60(int limit) {
        const int cacheSize = 3_000_000;
        int[] cache = new int[cacheSize];

        int count = 0;
        for (int n = 1; n < limit; n++)
            if (ChainLength(n, cache) == 60)
                count++;
        return count;
    }

    private static int ChainLength(int start, int[] cache) {
        var path = new List<int>(64);
        var indexIn = new Dictionary<long, int>();
        long current = start;
        int step = 0;
        while (true) {
            if (current < cache.Length && cache[(int)current] > 0) {
                int total = step + cache[(int)current];
                // backfill only values NOT in the chain starting at current — but since cache positions are by value
                // and chain from each path[i] is path[i..] ∪ (tail of cache[current]), we cannot cheaply prove disjointness.
                // Backfill only path[0] safely (total chain length is exact).
                if (start < cache.Length) cache[start] = total;
                return total;
            }
            if (indexIn.TryGetValue(current, out int firstIdx)) {
                int total = path.Count;
                if (start < cache.Length) cache[start] = total;
                return total;
            }
            indexIn[current] = step;
            path.Add((int)Math.Min(current, int.MaxValue));
            step++;
            current = DigitFactorialSum(current);
        }
    }

    private static long DigitFactorialSum(long n) {
        long sum = 0;
        while (n > 0) { sum += Fact[n % 10]; n /= 10; }
        return sum;
    }
}
