namespace Project_Euler.Problems;

public class Problem102 : Problem {
    public override object Solve() {
        return CountTrianglesContainingOrigin();
    }

    // Triangle contains origin iff the three cross products (sign) from each edge to origin agree.
    private int CountTrianglesContainingOrigin() {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "p102_triangles.txt");
        int count = 0;
        foreach (string line in File.ReadAllLines(path)) {
            string[] p = line.Split(',');
            int ax = int.Parse(p[0]), ay = int.Parse(p[1]);
            int bx = int.Parse(p[2]), by = int.Parse(p[3]);
            int cx = int.Parse(p[4]), cy = int.Parse(p[5]);
            long d1 = Sign(ax, ay, bx, by);
            long d2 = Sign(bx, by, cx, cy);
            long d3 = Sign(cx, cy, ax, ay);
            bool hasNeg = d1 < 0 || d2 < 0 || d3 < 0;
            bool hasPos = d1 > 0 || d2 > 0 || d3 > 0;
            if (!(hasNeg && hasPos)) count++;
        }
        return count;
    }

    private static long Sign(int x1, int y1, int x2, int y2) {
        return (long)x1 * y2 - (long)x2 * y1; // cross with origin — twice the signed area of O, P1, P2
    }
}
