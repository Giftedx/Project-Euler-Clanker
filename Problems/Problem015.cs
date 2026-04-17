namespace Project_Euler.Problems;

public class Problem015 : Problem {
    public override object Solve() {
        return LatticePaths(20, 20);
    }

    private long LatticePaths(int rows, int cols) {
        // C(rows+cols, rows) computed incrementally — always exact since running C(n,k) is integral
        long result = 1;
        for (int i = 1; i <= rows; i++)
            result = result * (cols + i) / i;
        return result;
    }
}
