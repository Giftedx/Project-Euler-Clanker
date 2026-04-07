namespace Project_Euler.Problems;

public class Problem050 : Problem {
    private const int Limit = 1000000;
    private readonly bool[] _isPrime = Library.GetSieve(Limit);

    public override object Solve() {
        return ConsecutivePrimeSumBelow(Limit);
    }

    private long ConsecutivePrimeSumBelow(int n) {
        // Build list of primes up to n
        var primes = new List<int>();
        for (int i = 2; i < n; i++) {
            if (_isPrime[i]) primes.Add(i);
        }

        // Build prefix sums for O(1) range sum queries
        long[] prefixSum = new long[primes.Count + 1];
        for (int i = 0; i < primes.Count; i++) {
            prefixSum[i + 1] = prefixSum[i] + primes[i];
        }

        int bestLength = 0;
        long bestPrime = 0;

        for (int start = 0; start < primes.Count; start++) {
            for (int end = start + bestLength + 1; end <= primes.Count; end++) {
                long sum = prefixSum[end] - prefixSum[start];
                if (sum >= n) break;
                if (sum > 0 && sum < _isPrime.Length && _isPrime[(int)sum]) {
                    int length = end - start;
                    if (length > bestLength) {
                        bestLength = length;
                        bestPrime = sum;
                    }
                }
            }
        }

        return bestPrime;
    }
}
