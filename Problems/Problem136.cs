namespace Project_Euler.Problems;

public class Problem136 : Problem {
    public override object Solve() {
        return CountNWithExactly(1, 50_000_000);
    }

    // Same equation as P135. Count n with exactly one (a, d) factorization.
    private int CountNWithExactly(int target, int limit) {
        byte[] counts = new byte[limit];
        for (int a = 1; a < limit; a++) {
            for (int d = (a + 4) / 5; d < a; d++) {
                int n = a * (4 * d - a);
                if (n <= 0) continue;
                if (n >= limit) break;
                if (counts[n] < 3) counts[n]++;
            }
        }
        int total = 0;
        for (int i = 1; i < limit; i++) if (counts[i] == target) total++;
        return total;
    }
}
