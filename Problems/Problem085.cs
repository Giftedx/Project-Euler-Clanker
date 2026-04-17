namespace Project_Euler.Problems;

public class Problem085 : Problem {
    public override object Solve() {
        return ClosestRectangleArea();
    }

    // Rectangles in an m x n grid = C(m+1,2) * C(n+1,2) = m(m+1)n(n+1)/4.
    // Search m,n up to a bound where product passes 2_000_000, keep nearest.
    private int ClosestRectangleArea() {
        const int target = 2_000_000;
        long bestDiff = long.MaxValue;
        int bestArea = 0;

        for (int m = 1; m < 200; m++) {
            for (int n = m; n < 200; n++) {
                long count = (long)m * (m + 1) * n * (n + 1) / 4;
                long diff = Math.Abs(count - target);
                if (diff >= bestDiff) {
                    if (count > target) break;
                    continue;
                }
                bestDiff = diff;
                bestArea = m * n;
            }
        }
        return bestArea;
    }
}
