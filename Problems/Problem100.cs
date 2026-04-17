namespace Project_Euler.Problems;

public class Problem100 : Problem {
    public override object Solve() {
        return FirstArrangementOver(1_000_000_000_000L);
    }

    // b/n * (b-1)/(n-1) = 1/2  ->  2*b*(b-1) = n*(n-1).
    // Substitute x = 2b-1, y = 2n-1 => y^2 - 2*x^2 = -1 (negative Pell).
    // Fundamental (y, x) = (1, 1); next via (y, x) -> (3y + 4x, 2y + 3x).
    private long FirstArrangementOver(long totalMin) {
        long y = 1, x = 1;
        while (true) {
            (y, x) = (3 * y + 4 * x, 2 * y + 3 * x);
            long n = (y + 1) / 2;
            long b = (x + 1) / 2;
            if (n > totalMin) return b;
        }
    }
}
