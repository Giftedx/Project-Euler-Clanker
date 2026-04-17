namespace Project_Euler.Problems;

public class Problem117 : Problem {
    public override object Solve() {
        return AllWaysMixed(50);
    }

    // Fill n-cell row with tiles of length 1 (grey), 2, 3, or 4 (colored).
    // f(n) = f(n-1) + f(n-2) + f(n-3) + f(n-4).
    private long AllWaysMixed(int n) {
        long[] f = new long[n + 1];
        f[0] = 1;
        for (int i = 1; i <= n; i++) {
            f[i] = f[i - 1];
            if (i >= 2) f[i] += f[i - 2];
            if (i >= 3) f[i] += f[i - 3];
            if (i >= 4) f[i] += f[i - 4];
        }
        return f[n];
    }
}
