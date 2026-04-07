namespace Project_Euler.Problems;

public class Problem050 : Problem {
    private const int Limit = 1000000;
    private readonly bool[] _isPrime = Library.GetSieve(Limit);

    public override object Solve() {
        return ConsecutivePrimeSumBelow(Limit);
    }

    private long ConsecutivePrimeSumBelow(int n) {
        var primes = new List<int>();
        for (int i = 2; i < n; i++) {
            if (_isPrime[i]) primes.Add(i);
        }

        // Build prefix sums
        long[] prefixSum = new long[primes.Count + 1];
        for (int i = 0; i < primes.Count; i++) {
            prefixSum[i + 1] = prefixSum[i] + primes[i];
        }

        // Search by decreasing length for early termination
        for (int length = primes.Count; length >= 2; length--) {
            for (int start = 0; start + length <= primes.Count; start++) {
                long sum = prefixSum[start + length] - prefixSum[start];
                if (sum >= n) break;
                if (_isPrime[(int)sum]) return sum;
            }
        }

        return 0;
    }
}
