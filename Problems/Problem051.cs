namespace Project_Euler.Problems;

public class Problem051 : Problem {
    private const int Limit = 1000000;
    private readonly bool[] _isPrime;
    private readonly List<int> _primes;

    public Problem051() {
        Library.SieveOfEratosthenes(Limit, out _isPrime);
        // Collect 6-digit primes only
        _primes = new List<int>();
        for (int i = Limit / 10; i < Limit; i++) {
            if (_isPrime[i]) _primes.Add(i);
        }
    }

    public override object Solve() {
        return EightPrimeFamily(8);
    }

    private static readonly int[] Pow10 = [1, 10, 100, 1000, 10000, 100000, 1000000];

    private int EightPrimeFamily(int target) {
        const int lowerBound = Limit / 10;
        int numDigits = (int)Math.Log10(Limit);
        var masks = GenerateMasks(numDigits, 3);

        foreach (int p in _primes) {
            foreach (int mask in masks) {
                int count = 0;
                int min = Limit + 1;

                for (int i = 0; i < 10; ++i) {
                    int newNum = Replace(p, mask, i);

                    if (newNum < lowerBound) continue;

                    if (_isPrime[newNum]) {
                        min = Math.Min(min, newNum);
                        ++count;
                    } else if (10 - i - 1 + count < target) break;
                }

                if (count == target) return min;
            }
        }

        return 0;
    }

    private static int Replace(int prime, int mask, int replacementDigit) {
        int result = 0;
        int shift = 0;

        while (prime > 0) {
            result += (mask & 1) == 1
                ? Pow10[shift] * replacementDigit
                : Pow10[shift] * (prime % 10);

            ++shift;
            mask >>= 1;
            prime /= 10;
        }

        return result;
    }

    private static List<int> GenerateMasks(int numDigits, int maxSetBits) {
        var masks = new List<int>();
        for (int mask = 1; mask < 1 << numDigits; ++mask)
            if (int.PopCount(mask) <= maxSetBits) masks.Add(mask);
        return masks;
    }
}
