namespace Project_Euler.Problems;

public class Problem070 : Problem {
    public override object Solve() {
        return TotientPermutationMinRatio(10_000_000);
    }

    // Minimizing n/phi(n) with phi(n) a digit permutation of n.
    // Sieve smallest prime factor, compute phi via factorization.
    // Minimum ratio wants few / large prime factors — focus attention there but still scan all n.
    private long TotientPermutationMinRatio(int limit) {
        int[] spf = BuildSmallestPrimeFactors(limit);

        double bestRatio = double.MaxValue;
        int bestN = 0;

        for (int n = 2; n < limit; n++) {
            int phi = Totient(n, spf);
            if (!IsDigitPermutation(n, phi)) continue;

            double ratio = (double)n / phi;
            if (ratio >= bestRatio) continue;
            bestRatio = ratio;
            bestN = n;
        }
        return bestN;
    }

    private static int[] BuildSmallestPrimeFactors(int limit) {
        var spf = new int[limit];
        for (int i = 2; i < limit; i++) {
            if (spf[i] != 0) continue;
            for (long j = i; j < limit; j += i) {
                if (spf[j] == 0) spf[j] = i;
            }
        }
        return spf;
    }

    private static int Totient(int n, int[] spf) {
        int result = n;
        while (n > 1) {
            int p = spf[n];
            result -= result / p;
            while (n % p == 0) n /= p;
        }
        return result;
    }

    private static bool IsDigitPermutation(int a, int b) {
        Span<int> counts = stackalloc int[10];
        while (a > 0) { counts[a % 10]++; a /= 10; }
        while (b > 0) { counts[b % 10]--; b /= 10; }
        for (int i = 0; i < 10; i++)
            if (counts[i] != 0) return false;
        return true;
    }
}
