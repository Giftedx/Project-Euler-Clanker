namespace Project_Euler.Problems;

public class Problem109 : Problem {
    public override object Solve() {
        return CountCheckoutsBelow100();
    }

    // Checkout = N darts (1-3) whose sum = S, last dart double. Enumerate possible darts:
    // singles 1..20 + 25, doubles 1..20 + 25 (outer bull counts as double), triples 1..20.
    // Distinctness is by (multiset of first N-1) x final-double. Two-dart & three-dart cases.
    private int CountCheckoutsBelow100() {
        var all = new List<int>();
        for (int i = 1; i <= 20; i++) all.Add(i);
        all.Add(25);
        for (int i = 1; i <= 20; i++) all.Add(2 * i);
        all.Add(50);
        for (int i = 1; i <= 20; i++) all.Add(3 * i);
        // all = all possible single-dart values, but there are duplicates (e.g., 20 as single, 10 as double = 20?). Actually 20 appears as single-20 and double-10: both are 20 but different "darts" — keep distinct as indices.

        // Reconstruct with dart identities
        var darts = new List<(int val, string kind)>();
        for (int i = 1; i <= 20; i++) darts.Add((i, "S" + i));
        darts.Add((25, "S25"));
        for (int i = 1; i <= 20; i++) darts.Add((2 * i, "D" + i));
        darts.Add((50, "D25"));
        for (int i = 1; i <= 20; i++) darts.Add((3 * i, "T" + i));

        // Doubles only
        var doubles = new List<(int val, string kind)>();
        for (int i = 1; i <= 20; i++) doubles.Add((2 * i, "D" + i));
        doubles.Add((50, "D25"));

        int count = 0;
        for (int S = 2; S < 100; S++) {
            // 1-dart checkout: final is double with value S
            foreach (var d in doubles) if (d.val == S) count++;
            // 2-dart checkout: dart1 + doubleDart = S
            for (int i = 0; i < darts.Count; i++)
                foreach (var d in doubles)
                    if (darts[i].val + d.val == S) count++;
            // 3-dart checkout: darts i <= j (unordered first two) + doubleDart = S
            for (int i = 0; i < darts.Count; i++)
                for (int j = i; j < darts.Count; j++)
                    foreach (var d in doubles)
                        if (darts[i].val + darts[j].val + d.val == S) count++;
        }
        return count;
    }
}
