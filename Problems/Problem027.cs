namespace Project_Euler.Problems;

public class Problem027 : Problem {
    private const int Limit = 1000;
    private readonly bool[] _isPrime = Library.GetSieve(Limit);

    public override object Solve() {
        return CoefficientProduct(Limit);
    }

    private int CoefficientProduct(int limit) {
        int bestA = 0, bestB = 0, maxLength = 0;
        for (int b = 3; b < limit; b += 2) {
            if (!_isPrime[b]) continue;
            for (int a = -limit + 1; a < limit; a += 2) {
                int n = 0;
                while (true) {
                    int val = n * n + a * n + b;
                    if (val < 0 || val >= Limit || !_isPrime[val]) break;
                    n++;
                }

                if (n <= maxLength) continue;
                maxLength = n;
                bestA = a;
                bestB = b;
            }
        }

        return bestA * bestB;
    }
}