using System.Numerics;

namespace Project_Euler.Problems;

public class Problem016 : Problem {
    public override object Solve() {
        return PowerDigitSum(2, 1000);
    }

    private int PowerDigitSum(int n, int exponent) {
        var digits = BigInteger.Pow(n, exponent);
        return Library.SumDigits(digits);
    }
}