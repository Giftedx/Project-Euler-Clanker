namespace Project_Euler.Problems;

public class Problem135 : Problem {
    public override object Solve() {
        return CountNWithExactly(10, 1_000_000);
    }

    // x^2 - y^2 - z^2 = n with x > y > z > 0 in AP. Let x = a+d, y = a, z = a - d => x-y-z = d-a.
    // Substitute: (a+d)^2 - a^2 - (a-d)^2 = 4ad - a^2 = a(4d - a) = n.
    // For each n, count positive (a, d) with d > a/4 (so 4d-a > 0) and a-d > 0 (so d < a) and a > 0.
    // Actually need z > 0 => a > d; and d > 0.
    // Iterate over divisor pairs n = a * m where m = 4d - a, so d = (m + a) / 4 must be integer, 0 < d < a.
    private int CountNWithExactly(int target, int limit) {
        int[] counts = new int[limit];
        for (int a = 1; a < limit; a++) {
            for (int d = (a + 4) / 5; d < a; d++) {
                int n = a * (4 * d - a);
                if (n <= 0) continue;
                if (n >= limit) break;
                counts[n]++;
            }
        }
        int total = 0;
        for (int i = 1; i < limit; i++) if (counts[i] == target) total++;
        return total;
    }
}
