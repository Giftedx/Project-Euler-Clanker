using System.Numerics;

namespace Project_Euler.Problems;

public class Problem080 : Problem {
    public override object Solve() {
        return SumOfDigitalSums100Digits();
    }

    // sqrt(N) to K decimal digits: work in BigInteger with N scaled by 10^(2K-2),
    // take integer square root — that's the first K digits (integer + (K-1) decimals).
    private int SumOfDigitalSums100Digits() {
        const int digits = 100;
        BigInteger scale = BigInteger.Pow(10, 2 * (digits - 1));
        int total = 0;
        for (int n = 2; n <= 100; n++) {
            int s = (int)Math.Sqrt(n);
            if (s * s == n) continue;
            BigInteger root = IntegerSqrt(new BigInteger(n) * scale);
            // root now has `digits` digits; sum them.
            while (root > 0) {
                total += (int)(root % 10);
                root /= 10;
            }
        }
        return total;
    }

    private static BigInteger IntegerSqrt(BigInteger n) {
        if (n < 2) return n;
        BigInteger x = BigInteger.One << (int)((n.GetBitLength() + 1) / 2);
        while (true) {
            BigInteger y = (x + n / x) >> 1;
            if (y >= x) return x;
            x = y;
        }
    }
}
