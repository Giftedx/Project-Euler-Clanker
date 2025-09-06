using System.Numerics;

namespace Project_Euler.Problems;

public class Problem056 : Problem {
    public override object Solve() {
        return PowerfulDigitSum();
    }

    private int PowerfulDigitSum() {
        int maxDigitSum = int.MinValue;
        for (int a = 99; a < 100; a++) {
            for (int b = 90; b < 100; b++) {
                BigInteger power = BigInteger.Pow(a, b);
                int digitSum = Library.SumDigits(power);
                maxDigitSum = Math.Max(maxDigitSum, digitSum);
            }
        }

        return maxDigitSum;
    }
}