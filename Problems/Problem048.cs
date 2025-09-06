using System.Numerics;

namespace Project_Euler.Problems;

public class Problem048 : Problem {
    public override object Solve() {
        return SelfPowSumLastTen();
    }

    private BigInteger SelfPowSumLastTen() {
        BigInteger sum = 0;
        var mod = BigInteger.Pow(10, 10);
        for (int i = 1; i <= 1000; i++)
            sum += BigInteger.ModPow(i, i, mod);
        return sum % mod;
    }
}