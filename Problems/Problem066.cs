using System.Numerics;

namespace Project_Euler.Problems;

public class Problem066 : Problem {
    public override object Solve() {
        return MaxPellD(1000);
    }

    // For non-square D, the fundamental solution (x,y) of x^2 - D*y^2 = 1 is the convergent
    // p/q of sqrt(D) at the index where the continued fraction period first ends (or twice
    // that index if the period is odd). We track convergents with BigInteger since x can be huge.
    private int MaxPellD(int limit) {
        BigInteger bestX = 0;
        int bestD = 0;

        for (int D = 2; D <= limit; D++) {
            int a0 = (int)Math.Sqrt(D);
            if (a0 * a0 == D) continue;

            int m = 0, d = 1, a = a0;
            BigInteger pPrev = 1, p = a0;
            BigInteger qPrev = 0, q = 1;

            while (true) {
                m = d * a - m;
                d = (D - m * m) / d;
                a = (a0 + m) / d;

                (pPrev, p) = (p, a * p + pPrev);
                (qPrev, q) = (q, a * q + qPrev);

                if (p * p - (BigInteger)D * q * q == 1) break;
            }

            if (p <= bestX) continue;
            bestX = p;
            bestD = D;
        }
        return bestD;
    }
}
