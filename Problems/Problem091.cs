namespace Project_Euler.Problems;

public class Problem091 : Problem {
    public override object Solve() {
        return CountRightTriangles(50);
    }

    // Classify by position of right angle:
    //  - at origin O: Q1 on x-axis, Q2 on y-axis => N*N such triangles (each non-origin pair)
    //  - at non-origin vertex P=(x,y) with P != O: count Q such that OP perpendicular QP.
    //    Slope of OP is y/x; Q lies on line through P with slope -x/y. Count integer Q != O, P in grid.
    private int CountRightTriangles(int N) {
        int count = N * N; // right angle at origin: pick one axis-aligned point per axis

        for (int x = 0; x <= N; x++) {
            for (int y = 0; y <= N; y++) {
                if (x == 0 && y == 0) continue;
                if (x == 0 || y == 0) {
                    // Right angle at P on axis — but then Q would be on x/y axis at same coord; degenerate or already counted.
                    // Actually if P is on x-axis, right angle at P means Q is directly above/below P.
                    // Count integer Q with x-coord = x, y-coord in [1..N]. Same for y-axis.
                    count += N;
                    continue;
                }
                // General case: P=(x,y). Line perpendicular at P has direction (-y, x) normalized.
                // Step = gcd(x, y); Q = P + k*(-y/g, x/g) for integer k, with Q in [0..N]^2 and Q != O, P.
                int g = Library.Gcd(x, y);
                int dx = -y / g, dy = x / g;
                for (int k = 1; ; k++) {
                    int qx = x + k * dx, qy = y + k * dy;
                    if (qx < 0 || qx > N || qy < 0 || qy > N) break;
                    count++;
                }
                for (int k = 1; ; k++) {
                    int qx = x - k * dx, qy = y - k * dy;
                    if (qx < 0 || qx > N || qy < 0 || qy > N) break;
                    count++;
                }
            }
        }
        return count;
    }
}
