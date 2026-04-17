namespace Project_Euler.Problems;

public class Problem023 : Problem {
    private const int Limit = 28123;
    private readonly int[] _properDivisorSum = new int[Limit];

    public Problem023() {
        for (int i = 1; i < Limit; i++)
        for (int j = 2 * i; j < Limit; j += i)
            _properDivisorSum[j] += i;
    }

    public override object Solve() {
        return SumOfNonAbundantBelow();
    }

    private int SumOfNonAbundantBelow() {
        int count = 0;
        for (int n = 12; n < Limit; n++)
            if (_properDivisorSum[n] > n) count++;

        int[] abundant = new int[count];
        int idx = 0;
        for (int n = 12; n < Limit; n++)
            if (_properDivisorSum[n] > n)
                abundant[idx++] = n;

        bool[] isAbundantSum = new bool[Limit];
        for (int i = 0; i < abundant.Length; i++) {
            int ai = abundant[i];
            if (ai + ai >= Limit) break;
            for (int j = i; j < abundant.Length; j++) {
                int s = ai + abundant[j];
                if (s >= Limit) break;
                isAbundantSum[s] = true;
            }
        }

        int total = 0;
        for (int i = 0; i < Limit; i++)
            if (!isAbundantSum[i])
                total += i;
        return total;
    }
}
