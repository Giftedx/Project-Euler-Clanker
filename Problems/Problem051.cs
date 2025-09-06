namespace Project_Euler.Problems;

public class Problem051 : Problem{
    private const int Limit = 1000000;
    private readonly bool[]  _isPrime;

    public Problem051() {
        Library.SieveOfEratosthenes(Limit, out _isPrime);
    }
    
    public override object Solve() {
        return EightPrimeFamily(8);
    }
    
    private readonly int[] _pow10 = [1, 10, 100, 1000, 10000, 100000, 1000000];

    private int EightPrimeFamily(int target) {
        const int lowerBound = Limit / 10;
        int numDigits = (int)Math.Log10(Limit);
        var masks = GenerateMasks(numDigits, 3);

        for (int p = 0; p < Limit; p++){
            if(!_isPrime[p])continue;
            if (p < lowerBound) continue;

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

    private int Replace(int prime, long mask, int replacementDigit) {
        int result = 0;
        int shift = 0;

        while (prime > 0) {
            result += (mask & 1) == 1 ? 
                _pow10[shift] * replacementDigit : 
                _pow10[shift] * (prime % 10);

            ++shift;
            mask >>= 1;
            prime /= 10;
        }

        return result;
    }

    private List<int> GenerateMasks(int numDigits, int maxSetBits) {
        var masks = new List<int>();
        for (int mask = 1; mask < 1 << numDigits; ++mask)
            if (CountBits(mask) <= maxSetBits) masks.Add(mask);
        return masks;
    }

    private int CountBits(int n) {
        int count = 0;
        while (n > 0) {
            count += n & 1;
            n >>= 1;
        }
        return count;
    }
}