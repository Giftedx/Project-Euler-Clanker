namespace Project_Euler.Problems;

public class Problem104 : Problem {
    public override object Solve() {
        return FirstPandigitalBothEnds();
    }

    // Maintain full F_k mod 10^9 (last 9 digits) and doubled-precision head via log identity.
    // Head: log10(F_k) - floor(log10(F_k)) gives fractional part; take top 9 leading digits.
    private int FirstPandigitalBothEnds() {
        long tail1 = 1, tail2 = 1; // F_{k-1}, F_k mod 10^9
        double logPhi = Math.Log10((1 + Math.Sqrt(5)) / 2);
        double logSqrt5 = Math.Log10(Math.Sqrt(5));

        for (int k = 3; ; k++) {
            long next = (tail1 + tail2) % 1_000_000_000L;
            tail1 = tail2; tail2 = next;

            if (!IsPandigital(tail2)) continue;

            double logF = k * logPhi - logSqrt5;
            double frac = logF - Math.Floor(logF);
            long head = (long)(Math.Pow(10, frac + 8));
            if (IsPandigital(head)) return k;
        }
    }

    private static bool IsPandigital(long n) {
        int mask = 0;
        for (int i = 0; i < 9; i++) {
            int d = (int)(n % 10);
            if (d == 0) return false;
            mask |= 1 << d;
            n /= 10;
        }
        return mask == 0x3FE;
    }
}
