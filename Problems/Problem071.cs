namespace Project_Euler.Problems;

public class Problem071 : Problem {
    public override object Solve() {
        return LargestLeftNeighborOf3Over7(1_000_000);
    }

    // Predecessor of 3/7 in Farey sequence with denominator limit L: among d <= L,
    // the best numerator is floor((3d - 1) / 7) and we maximize n/d by scanning d.
    // Equivalently, the answer comes from the largest d coprime to 7 near L; we track numerator directly.
    private int LargestLeftNeighborOf3Over7(int limit) {
        int bestNum = 0, bestDen = 1;
        for (int d = 2; d <= limit; d++) {
            int n = (3 * d - 1) / 7;
            // Ensure strict inequality n/d < 3/7 — (3*d - 1)/7 guarantees this, unless 7 | d and the numerator hits 3d/7 then decremented.
            if (n * bestDen <= bestNum * d) continue;
            bestNum = n; bestDen = d;
        }
        return bestNum / Library.Gcd(bestNum, bestDen);
    }
}
