namespace Project_Euler.Problems;

public class Problem086 : Problem {
    public override object Solve() {
        return SmallestMOverMillion();
    }

    // For cuboid a<=b<=c, shortest surface route length = sqrt((a+b)^2 + c^2).
    // With c = max dimension = M, iterate M upward; for each M, count (a,b) with a+b from 2..2M
    // producing integer sqrt. The count for fixed s=a+b and c is the # of valid (a,b) pairs with a<=b<=c.
    private int SmallestMOverMillion() {
        int count = 0;
        for (int M = 1; ; M++) {
            for (int s = 2; s <= 2 * M; s++) {
                int sq = s * s + M * M;
                int r = (int)Math.Sqrt(sq);
                if ((long)r * r != sq) continue;
                // (a,b) with a<=b, a+b=s, a>=1, b<=M
                int aMin = Math.Max(1, s - M);
                int aMax = s / 2;
                if (aMax >= aMin) count += aMax - aMin + 1;
            }
            if (count > 1_000_000) return M;
        }
    }
}
