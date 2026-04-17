namespace Project_Euler.Problems;

public class Problem095 : Problem {
    public override object Solve() {
        return SmallestMemberOfLongestAmicableChain(1_000_000);
    }

    // Sieve sums of proper divisors below limit. Then for each n, walk the chain;
    // discard if it ever exceeds the limit or revisits a value outside the chain start.
    private int SmallestMemberOfLongestAmicableChain(int limit) {
        int[] sum = new int[limit + 1];
        for (int i = 1; i <= limit / 2; i++)
            for (int j = 2 * i; j <= limit; j += i)
                sum[j] += i;

        int[] chainLen = new int[limit + 1];
        int bestLen = 0, bestMin = 0;

        for (int start = 2; start <= limit; start++) {
            if (chainLen[start] != 0) continue;
            var path = new List<int>();
            var indexIn = new Dictionary<int, int>();
            int current = start;
            int pos = 0;
            while (true) {
                if (current > limit || current < 1) { break; }
                if (indexIn.TryGetValue(current, out int first)) {
                    int chainStart = first;
                    int loopLen = pos - chainStart;
                    if (loopLen > bestLen) {
                        bestLen = loopLen;
                        bestMin = int.MaxValue;
                        for (int k = chainStart; k < pos; k++)
                            if (path[k] < bestMin) bestMin = path[k];
                    }
                    break;
                }
                indexIn[current] = pos;
                path.Add(current);
                pos++;
                current = sum[current];
            }
            // Mark visited so we don't re-walk (chain length of nothing useful to cache beyond "visited")
            foreach (var v in path) if (v <= limit) chainLen[v] = 1;
        }
        return bestMin;
    }
}
