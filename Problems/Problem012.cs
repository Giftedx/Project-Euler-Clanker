namespace Project_Euler.Problems;

public class Problem012 : Problem {
    // Precompute smallest prime factor for fast factorization
    private const int Limit = 100000;
    private readonly int[] _spf = new int[Limit];

    public Problem012() {
        for (int i = 2; i < Limit; i++) {
            if (_spf[i] != 0) continue;
            for (int j = i; j < Limit; j += i) {
                if (_spf[j] == 0) _spf[j] = i;
            }
        }
    }

    public override object Solve() {
        return HighlyDivisibleTriangle(500);
    }

    private long HighlyDivisibleTriangle(int minDivisors) {
        int n = 1;
        while (true) {
            int divisorsCount;
            if (n % 2 == 0) divisorsCount = Tau(n >> 1) * Tau(n + 1);
            else divisorsCount = Tau(n) * Tau((n + 1) >> 1);

            if (divisorsCount > minDivisors)
                return ((long)n * (n + 1)) >> 1;

            n++;
        }
    }

    private int Tau(int num) {
        if (num < 2) return 1;
        int count = 1;
        while (num > 1) {
            int p = _spf[num];
            int exp = 0;
            while (num % p == 0) {
                num /= p;
                exp++;
            }
            count *= (exp + 1);
        }
        return count;
    }
}
