namespace Project_Euler.Problems;

public class Problem039 : Problem {
    public override object Solve() {
        return MaxTrianglePerimeter();
    }

    private int MaxTrianglePerimeter() {
        const int limit = 1000;
        int[] perimeters = new int[limit + 1];
        int limitSqrt = (int)Math.Sqrt(limit);

        for (int m = 2; m < limitSqrt; m++)
        for (int n = 1; n < m; n++) {
            if ((m - n) % 2 == 0) continue;
            if (Library.Gcd(m, n) != 1) continue;

            int a = m * m - n * n;
            int b = 2 * m * n;
            int c = m * m + n * n;
            int p = a + b + c;

            for (int k = 1; k * p <= limit; k++)
                perimeters[k * p]++;
        }

        int maxP = 0, maxCount = 0;
        for (int p = 0; p <= limit; p++) {
            if (perimeters[p] <= maxCount) continue;
            maxCount = perimeters[p];
            maxP = p;
        }

        return maxP;
    }
}