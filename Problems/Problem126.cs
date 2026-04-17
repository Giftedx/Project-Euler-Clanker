namespace Project_Euler.Problems;

public class Problem126 : Problem {
    public override object Solve() {
        return LeastNWithCount(1000);
    }

    // Cubes (a,b,c), layer k cubes = 2(ab + bc + ca) + 4(a + b + c - 3)(k - 1) + 4(k - 1)^2 * ... for k>=2,
    // or more cleanly: C(a,b,c,k) = 2(ab+bc+ca) + 4(k-1)(a+b+c) + 4(k-1)(k-2) for k>=1.
    // For k=1: 2(ab+bc+ca). We accumulate counts for values and find smallest value reaching target count.
    private int LeastNWithCount(int target) {
        const int limit = 20000;
        int[] count = new int[limit + 1];

        for (int a = 1; a < limit; a++) {
            if (2 * a * a >= limit) break;
            for (int b = a; b < limit; b++) {
                if (2 * (a * b + b * b + a * b) >= limit && 2 * a * b >= limit) break;
                for (int c = b; ; c++) {
                    long baseLayer = 2L * (a * b + b * c + c * a);
                    if (baseLayer >= limit) break;
                    for (int k = 1; ; k++) {
                        long layer = k == 1
                            ? baseLayer
                            : baseLayer + 4L * (k - 1) * (a + b + c) + 4L * (k - 1) * (k - 2);
                        if (layer >= limit) break;
                        count[(int)layer]++;
                    }
                }
            }
        }
        for (int n = 1; n <= limit; n++) if (count[n] == target) return n;
        return -1;
    }
}
