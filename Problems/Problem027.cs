namespace Project_Euler.Problems;

public class Problem027 : Problem {
    // n²+an+b can produce values up to ~1000000, extend sieve
    private const int SieveLimit = 1_000_000;
    private readonly bool[] _isPrime = Library.GetSieve(SieveLimit);

    public override object Solve() {
        return CoefficientProduct(1000);
    }

    private int CoefficientProduct(int limit) {
        // Collect primes up to limit for b (b must be prime since n=0 gives b)
        var bPrimes = new List<int>();
        for (int i = 2; i < limit; i++) {
            if (_isPrime[i]) bPrimes.Add(i);
        }

        int bestA = 0, bestB = 0, maxLength = 0;

        foreach (int b in bPrimes) {
            for (int a = -limit + 1; a < limit; a += 2) {
                int n = 0;
                while (true) {
                    int val = n * n + a * n + b;
                    if (val < 2 || val >= SieveLimit || !_isPrime[val]) break;
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
