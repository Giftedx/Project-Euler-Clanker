using System.Numerics;

namespace Project_Euler.Problems;

public class Problem056 : Problem {
    public override object Solve() {
        return PowerfulDigitSum();
    }

    private int PowerfulDigitSum() {
        int best = 0;
        // Iterate from large a downward so best is set high early, maximizing pruning
        for (int a = 99; a >= 2; a--) {
            double log10a = Math.Log10(a);
            if (9.0 * 99 * log10a < best) break; // all smaller a will also fail

            BigInteger power = (BigInteger)a;
            for (int b = 2; b < 100; b++) {
                power *= a;
                if (9.0 * (b * log10a + 1) < best) continue;
                int digitSum = Library.SumDigits(power);
                if (digitSum > best) best = digitSum;
            }
        }
        return best;
    }
}