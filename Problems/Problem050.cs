namespace Project_Euler.Problems;

public class Problem050 : Problem {
    private const int Limit = 1000000;
    private readonly bool[] _isPrime = Library.GetSieve(Limit);

    public override object Solve() {
        return ConsecutivePrimeSumBelow(Limit);
    }

    private long ConsecutivePrimeSumBelow(int n) {
        // Build prefix sums directly — no List needed
        long[] prefix = new long[80000];
        int count = 0;
        for (int i = 2; i < n; i++) {
            if (!_isPrime[i]) continue;
            prefix[count + 1] = prefix[count] + i;
            count++;
        }

        // Max chain length: largest k where prefix[k] < n
        int maxLen = count;
        while (prefix[maxLen] >= n) maxLen--;

        for (int len = maxLen; len >= 2; len--) {
            for (int s = 0; s + len <= count; s++) {
                long sum = prefix[s + len] - prefix[s];
                if (sum >= n) break;
                if (_isPrime[(int)sum]) return sum;
            }
        }
        return 0;
    }
}
