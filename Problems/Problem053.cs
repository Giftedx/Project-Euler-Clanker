namespace Project_Euler.Problems;

public class Problem053 : Problem {
    private const int Limit = 1000000;
    public override object Solve() {
        return CombinatoricSelection(1, 100);
    }

    private int CombinatoricSelection(int lower, int upper) {
        int count = 0;
        for (int n = lower; n <= upper; n++) {
            // C(n,r) is symmetric and unimodal — find first r exceeding limit
            for (int r = 1; r <= n / 2; r++) {
                if (ExceedsLimit(n, r)) {
                    // All C(n,r) through C(n,n-r) exceed the limit
                    count += n - 2 * r + 1;
                    break;
                }
            }
        }
        return count;
    }

    private bool ExceedsLimit(int n, int r) {
        long result = 1;
        for (int i = 1; i <= r; i++) {
            result = result * (n - r + i) / i;
            if (result > Limit) return true;
        }
        return false;
    }
}