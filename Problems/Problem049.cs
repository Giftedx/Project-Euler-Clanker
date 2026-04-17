namespace Project_Euler.Problems;

public class Problem049 : Problem {
    private const int Limit = 10000;
    private readonly bool[] _isPrime = Library.GetSieve(Limit);

    public override object Solve() {
        return OtherPrimePermuteConcat();
    }

    private long OtherPrimePermuteConcat() {
        const int gap = 3330;
        for (int i = 1001; i < Limit; i += 2) {
            if (!_isPrime[i]) continue;

            int i1 = i + gap;
            int i2 = i1 + gap;

            if (i1 < Limit && _isPrime[i1] && SameDigits(i, i1) &&
                i2 < Limit && _isPrime[i2] && SameDigits(i, i2) &&
                i != 1487 && i != 4817 && i != 8147)
                return (long)i * 100_000_000 + (long)i1 * 10_000 + i2;
        }

        return -1;
    }

    private bool SameDigits(int n, int m) {
        int[] counter = new int[10];

        while (n > 0) {
            counter[n % 10]++;
            counter[m % 10]--;
            n /= 10;
            m /= 10;
        }

        foreach (int t in counter)
            if (t != 0)
                return false;
        return true;
    }
}