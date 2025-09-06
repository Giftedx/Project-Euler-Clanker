namespace Project_Euler.Problems;

public class Problem053 : Problem {
    private const int Limit = 1000000;
    public override object Solve() {
        return CombinatoricSelection(1, 100);
    }

    private int CombinatoricSelection(int lower, int upper) {
        int count = 0;
        for (int n = lower; n <= upper; n++)
            for (int r = 0; r <= n; r++)
                if (BinomialCoefficient(n, r)) count++;
        return count;
    }

    private bool BinomialCoefficient(int n, int r) {
        if (r > n - r) r = n - r;
        long result = 1;

        for (int i = 1; i <= r; i++) {
            result *= n - r + i;
            result /= i;

            if (result > Limit) return true;
        }

        return false;
    }
}