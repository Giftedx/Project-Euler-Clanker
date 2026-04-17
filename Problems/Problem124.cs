namespace Project_Euler.Problems;

public class Problem124 : Problem {
    public override object Solve() {
        return NthOrdered(10_000, 100_000);
    }

    // Compute rad(n) = product of distinct prime factors via sieve.
    // Sort indices 1..N by (rad, n); return the k-th.
    private int NthOrdered(int k, int limit) {
        int[] rad = new int[limit + 1];
        for (int i = 0; i <= limit; i++) rad[i] = 1;
        for (int p = 2; p <= limit; p++) {
            if (rad[p] != 1) continue;
            for (int j = p; j <= limit; j += p) rad[j] *= p;
        }

        int[] idx = new int[limit];
        for (int i = 0; i < limit; i++) idx[i] = i + 1;
        Array.Sort(idx, (a, b) => {
            int cmp = rad[a].CompareTo(rad[b]);
            return cmp != 0 ? cmp : a.CompareTo(b);
        });
        return idx[k - 1];
    }
}
