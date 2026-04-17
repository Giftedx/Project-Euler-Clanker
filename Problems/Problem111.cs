namespace Project_Euler.Problems;

public class Problem111 : Problem {
    public override object Solve() {
        return SumPrimesWithMostRepeatedDigits(10);
    }

    // For each digit d, find M(n, d) = max count of digit d in any n-digit prime, then S(n, d) = sum of those primes.
    // Strategy: for n=10, iterate k = n, n-1, ..., 0 repeats. For each k, place d in k positions; fill the remaining
    // n-k with all digit patterns; check primality. Return sum over d of S(n,d).
    private long SumPrimesWithMostRepeatedDigits(int n) {
        long total = 0;
        for (int d = 0; d <= 9; d++) {
            for (int k = n; k >= 0; k--) {
                long sumAtK = SumPrimesWithExactlyKCopies(n, d, k);
                if (sumAtK == 0) continue;
                total += sumAtK;
                break;
            }
        }
        return total;
    }

    private long SumPrimesWithExactlyKCopies(int n, int d, int k) {
        // Choose k positions for digit d among n positions; remaining n-k positions can be any digit 0..9 != d.
        int otherPositions = n - k;
        long sum = 0;
        int[] positions = new int[k];
        // Generate all k-position subsets and fill remaining slots with all 10^(n-k) possibilities.
        foreach (var posSet in Combinations(n, k)) {
            bool[] isD = new bool[n];
            foreach (int p in posSet) isD[p] = true;
            long totalOthers = 1;
            for (int i = 0; i < otherPositions; i++) totalOthers *= 9;
            for (long combo = 0; combo < totalOthers; combo++) {
                long val = 0;
                long rem = combo;
                int idx = 0;
                bool leadingZero = false;
                for (int i = 0; i < n; i++) {
                    int digit;
                    if (isD[i]) digit = d;
                    else {
                        int c = (int)(rem % 9);
                        rem /= 9;
                        digit = c < d ? c : c + 1;
                        idx++;
                    }
                    if (i == 0 && digit == 0) { leadingZero = true; break; }
                    val = val * 10 + digit;
                }
                if (leadingZero) continue;
                if (IsPrimeLong(val)) sum += val;
            }
        }
        return sum;
    }

    private static IEnumerable<int[]> Combinations(int n, int k) {
        int[] idx = new int[k];
        for (int i = 0; i < k; i++) idx[i] = i;
        while (true) {
            yield return (int[])idx.Clone();
            int i = k - 1;
            while (i >= 0 && idx[i] == n - k + i) i--;
            if (i < 0) yield break;
            idx[i]++;
            for (int j = i + 1; j < k; j++) idx[j] = idx[j - 1] + 1;
        }
    }

    private static bool IsPrimeLong(long n) {
        if (n < 2) return false;
        if (n < 4) return true;
        if ((n & 1) == 0) return false;
        if (n % 3 == 0) return n == 3;
        long limit = (long)Math.Sqrt(n);
        for (long i = 5; i <= limit; i += 6) {
            if (n % i == 0 || n % (i + 2) == 0) return false;
        }
        return true;
    }
}
