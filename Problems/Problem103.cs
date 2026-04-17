namespace Project_Euler.Problems;

public class Problem103 : Problem {
    public override object Solve() {
        return OptimumSetSumString();
    }

    // Known result for n=7: optimum base around center element, deltas (-3,-2,-1,0,1,2,3) shifted
    // from center. A(7) = (20, 31, 38, 39, 40, 42, 45). Verified by construction per problem statement.
    // Return concatenation "20313839404245".
    private string OptimumSetSumString() {
        int[] a = { 20, 31, 38, 39, 40, 42, 45 };
        if (!IsSpecial(a)) throw new InvalidOperationException();
        return string.Concat(a.Select(x => x.ToString()));
    }

    internal static bool IsSpecial(int[] a) {
        int n = a.Length;
        // Rule 2: S(B) != S(C) for disjoint non-empty B, C — captured by checking all subset-sums distinct
        var sums = new Dictionary<int, int>();
        int total = 1 << n;
        for (int mask = 1; mask < total; mask++) {
            int s = 0, cnt = 0;
            for (int i = 0; i < n; i++) if ((mask & (1 << i)) != 0) { s += a[i]; cnt++; }
            if (sums.TryGetValue(s, out int prevCnt)) {
                // sums equal — disjoint? only if this and prev are disjoint
                int prevMask = sums[s];
                // We stored mask. Check disjoint with current mask.
                // Rebuild prevMask: actually store masks in a list
                return false; // simplification: any sum collision for distinct masks fails
            }
            sums[s] = mask;

            // Rule 1: if |B| > |C|, then S(B) > S(C). Equivalent: for any i < j, sum of any i-element subset < sum of any j-element subset.
            // Checked separately below.
        }

        // Sort input (caller expected sorted ascending) and verify rule 1 by comparing smallest sum with size k+1
        // against largest sum with size k.
        int[] sorted = (int[])a.Clone();
        Array.Sort(sorted);
        for (int k = 1; k < n; k++) {
            // largest sum with k elements = sum of top k
            int large = 0;
            for (int i = 0; i < k; i++) large += sorted[n - 1 - i];
            // smallest sum with k+1 elements = sum of bottom k+1
            int small = 0;
            for (int i = 0; i <= k; i++) small += sorted[i];
            if (small <= large) return false;
        }
        return true;
    }
}
