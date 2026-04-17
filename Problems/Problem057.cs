using System.Numerics;

namespace Project_Euler.Problems;

public class Problem057 : Problem {
    public override object Solve() {
        return SquareConvergents();
    }

    private int SquareConvergents() {
        BigInteger n = 0;
        BigInteger d = 1;
        BigInteger pow10 = 10; // smallest power of 10 greater than d
        int count = 0;
        for (int i = 0; i < 1000; i++) {
            (n, d) = (d, d * 2 + n);
            // Advance threshold when d catches up
            while (pow10 <= d) pow10 *= 10;
            // Numerator (n+d) has more digits than denominator (d) iff n+d >= pow10
            if (n + d >= pow10) count++;
        }
        return count;
    }
}