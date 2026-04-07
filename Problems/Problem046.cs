namespace Project_Euler.Problems;

public class Problem046 : Problem {
    private const int Limit = 10000;
    private readonly bool[] _isPrime = Library.GetSieve(Limit);

    public override object Solve() {
        return DisproveGoldbach();
    }

    private int DisproveGoldbach() {
        for (int n = 9; n < Limit; n += 2) {
            if (_isPrime[n]) continue;
            if (!SatisfiesGoldbach(n)) return n;
        }
        return -1;
    }

    private bool SatisfiesGoldbach(int n) {
        for (int i = 1; 2 * i * i < n; i++) {
            int remainder = n - 2 * i * i;
            if (remainder > 0 && _isPrime[remainder]) return true;
        }
        return false;
    }
}
