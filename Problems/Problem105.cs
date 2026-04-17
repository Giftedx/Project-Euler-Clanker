namespace Project_Euler.Problems;

public class Problem105 : Problem {
    public override object Solve() {
        return SumOfSpecialSetSums();
    }

    private int SumOfSpecialSetSums() {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "p105_sets.txt");
        int total = 0;
        foreach (string line in File.ReadAllLines(path)) {
            int[] a = line.Split(',').Select(int.Parse).ToArray();
            if (IsSpecialSumSet(a)) total += a.Sum();
        }
        return total;
    }

    private static bool IsSpecialSumSet(int[] a) {
        int n = a.Length;
        int[] sorted = (int[])a.Clone();
        Array.Sort(sorted);
        // Rule 1: if subset size increases strictly, sum must strictly increase.
        for (int k = 1; k < n; k++) {
            int large = 0;
            for (int i = 0; i < k; i++) large += sorted[n - 1 - i];
            int small = 0;
            for (int i = 0; i <= k; i++) small += sorted[i];
            if (small <= large) return false;
        }
        // Rule 2: all non-empty subset sums are distinct.
        int total = 1 << n;
        var seen = new HashSet<int>();
        for (int mask = 1; mask < total; mask++) {
            int s = 0;
            for (int i = 0; i < n; i++) if ((mask & (1 << i)) != 0) s += sorted[i];
            if (!seen.Add(s)) return false;
        }
        return true;
    }
}
