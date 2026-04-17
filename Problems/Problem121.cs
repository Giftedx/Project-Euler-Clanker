namespace Project_Euler.Problems;

public class Problem121 : Problem {
    public override object Solve() {
        return MaxPrizeFund(15);
    }

    // P(blue on round k) = 1 / (k + 1). Let p[i] be P(i blues after rounds 1..n).
    // Prize fund = floor(1 / P(blues > reds)).
    private long MaxPrizeFund(int rounds) {
        double[] p = new double[rounds + 1];
        p[0] = 1;
        for (int k = 1; k <= rounds; k++) {
            double blue = 1.0 / (k + 1), red = k / (double)(k + 1);
            double[] next = new double[rounds + 1];
            for (int i = 0; i <= rounds; i++) {
                if (p[i] == 0) continue;
                next[i] += p[i] * red;
                if (i + 1 <= rounds) next[i + 1] += p[i] * blue;
            }
            p = next;
        }
        double winProb = 0;
        int majority = rounds / 2 + 1;
        for (int i = majority; i <= rounds; i++) winProb += p[i];
        return (long)Math.Floor(1.0 / winProb);
    }
}
