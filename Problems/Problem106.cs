namespace Project_Euler.Problems;

public class Problem106 : Problem {
    public override object Solve() {
        return CountPairsNeedingTest(12);
    }

    // For a strictly increasing sequence, rule 2 requires equality testing for disjoint pairs (B, C)
    // of equal size. Pairs where B dominates C elementwise auto-satisfy. The non-trivial pairs for size k
    // equal the k-th Catalan-triangle count: sum_{k>=2} C(2k, k) / (k+1) * C(n, 2k) — actually # of non-crossing
    // interleavings of 2k items into two ordered k-subsets that are NOT dominance-ordered.
    // Count = sum_{k=2..floor(n/2)} C(n, 2k) * (Cat(k) * k  — ...)
    // Simpler: enumerate 2k-subsets, count # of k-vs-k partitions that need a test.
    // A partition {A, B} needs a test iff NOT (A_i < B_i for all i) AND NOT (B_i < A_i for all i).
    private int CountPairsNeedingTest(int n) {
        int needed = 0;
        for (int k = 2; 2 * k <= n; k++) {
            int pairsPerSubset = NonDominatedPartitions(k);
            needed += Binomial(n, 2 * k) * pairsPerSubset;
        }
        return needed;
    }

    // Count unordered partitions of {1..2k} into two k-subsets that are NOT dominance-ordered.
    // Total unordered k-vs-k partitions = C(2k, k) / 2.
    // Dominance-ordered: from sorted order a_1 < a_2 < ... < a_{2k}, one choice where A_i < B_i for all i
    // corresponds to lattice paths with a certain property = Catalan number Cat(k).
    // So non-dominated = C(2k, k)/2 - Cat(k).
    private static int NonDominatedPartitions(int k) {
        int total = Binomial(2 * k, k) / 2;
        int catalan = Binomial(2 * k, k) / (k + 1);
        return total - catalan;
    }

    private static int Binomial(int n, int k) {
        if (k < 0 || k > n) return 0;
        if (k > n - k) k = n - k;
        long r = 1;
        for (int i = 0; i < k; i++) r = r * (n - i) / (i + 1);
        return (int)r;
    }
}
