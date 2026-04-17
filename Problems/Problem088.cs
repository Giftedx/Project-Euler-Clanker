namespace Project_Euler.Problems;

public class Problem088 : Problem {
    public override object Solve() {
        return SumOfMinProductSums(12_000);
    }

    // For each multiset of factors >= 2 with product P and sum S, k = (# factors) + (P - S)
    // because we can pad with ones. Enumerate factorizations with product up to 2*kMax.
    private int SumOfMinProductSums(int kMax) {
        int[] best = new int[kMax + 1];
        for (int i = 0; i <= kMax; i++) best[i] = int.MaxValue;

        void Recurse(int product, int sum, int numFactors, int minFactor) {
            int k = numFactors + product - sum;
            if (k > kMax) return;
            if (numFactors >= 2 && product < best[k]) best[k] = product;

            for (int f = minFactor; product * f <= 2 * kMax; f++)
                Recurse(product * f, sum + f, numFactors + 1, f);
        }

        Recurse(1, 0, 0, 2);

        var distinct = new HashSet<int>();
        for (int k = 2; k <= kMax; k++) distinct.Add(best[k]);
        int total = 0;
        foreach (int v in distinct) total += v;
        return total;
    }
}
