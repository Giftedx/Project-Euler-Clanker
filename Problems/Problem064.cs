namespace Project_Euler.Problems;

public class Problem064 : Problem {
    public override object Solve() {
        return OddPeriodSquareRoots(10000);
    }

    // Continued fraction of sqrt(n) via the m,d,a recurrence:
    //   a0 = floor(sqrt(n)); m0 = 0; d0 = 1.
    //   m' = d*a - m;  d' = (n - m'^2) / d;  a' = (a0 + m') / d'.
    // Period ends when the triple (m, d, a) repeats; equivalently when a' = 2*a0.
    private static int OddPeriodSquareRoots(int limit) {
        int count = 0;
        for (int n = 2; n <= limit; n++) {
            int a0 = (int)Math.Sqrt(n);
            if (a0 * a0 == n) continue;

            int m = 0, d = 1, a = a0;
            int period = 0;
            do {
                m = d * a - m;
                d = (n - m * m) / d;
                a = (a0 + m) / d;
                period++;
            } while (a != 2 * a0);

            if ((period & 1) == 1) count++;
        }
        return count;
    }
}
