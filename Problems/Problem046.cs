namespace Project_Euler.Problems;

public class Problem046 : Problem {
    public override object Solve() {
        return DisproveGoldbach();
    }

    private int DisproveGoldbach() {
        int i = 9;
        while (SatisfiesGoldbach(i)) i += 2;
        return i;
    }

    private bool SatisfiesGoldbach(int n) {
        if (n % 2 == 0 || Library.IsPrime(n)) return true;

        for (int i = 1; i * i * 2 < n; i++) {
            int remainder = n - 2 * i * i;
            if (Library.IsPrime(remainder)) return true;
        }

        return false;
    }
}