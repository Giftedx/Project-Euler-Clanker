namespace Project_Euler.Problems;

public class Problem125 : Problem {
    public override object Solve() {
        return SumPalindromeSumsOfSquares(100_000_000);
    }

    // For every starting i, accumulate i^2 + (i+1)^2 + ... until the running sum exceeds limit.
    // At each intermediate (length >= 2), check palindrome and add to a set.
    private long SumPalindromeSumsOfSquares(long limit) {
        var seen = new HashSet<long>();
        int maxI = (int)Math.Sqrt(limit);
        for (int i = 1; i <= maxI; i++) {
            long sum = (long)i * i;
            for (int j = i + 1; ; j++) {
                sum += (long)j * j;
                if (sum >= limit) break;
                if (Library.IsPalindrome(sum)) seen.Add(sum);
            }
        }
        long total = 0;
        foreach (long v in seen) total += v;
        return total;
    }
}
