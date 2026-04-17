namespace Project_Euler.Problems;

public class Problem110 : Problem {
    public override object Solve() {
        return SmallestNWithOver(4_000_000);
    }

    // Same form as P108 with larger threshold. Exhaust candidates by building n = prod p_i^{e_i}
    // in decreasing-exponent order using small primes. BFS over exponent tuples, prune by product.
    private long SmallestNWithOver(int threshold) {
        int[] primes = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47 };
        long best = long.MaxValue;

        void Recurse(int idx, int prevExp, long prod, long odd) {
            if ((odd + 1) / 2 > threshold) {
                if (prod < best) best = prod;
                return;
            }
            if (idx >= primes.Length) return;
            for (int e = 1; e <= prevExp; e++) {
                long nextProd = prod;
                for (int i = 0; i < e; i++) {
                    nextProd *= primes[idx];
                    if (nextProd >= best) return;
                }
                Recurse(idx + 1, e, nextProd, odd * (2L * e + 1));
            }
        }

        Recurse(0, 30, 1, 1);
        return best;
    }
}
