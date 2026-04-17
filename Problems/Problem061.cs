namespace Project_Euler.Problems;

public class Problem061 : Problem {
    public override object Solve() {
        return CyclicalFigurateSum();
    }

    // Generate 4-digit figurate numbers (1000–9999) for sides 3..8,
    // then DFS for an ordered cycle with one number per side whose last 2 digits
    // equal the next element's first 2 digits (and loop back to the first).
    private int CyclicalFigurateSum() {
        // buckets[s][prefix] = list of 4-digit figurates with shape s whose first two digits = prefix
        var buckets = new List<int>[9, 100];
        for (int s = 3; s <= 8; s++)
            for (int p = 0; p < 100; p++)
                buckets[s, p] = new List<int>();

        for (int s = 3; s <= 8; s++) {
            foreach (int v in Figurates(s)) {
                int prefix = v / 100;
                buckets[s, prefix].Add(v);
            }
        }

        // Fix shape 3 as starter — the chain is cyclic, so every solution contains a triangle and rotations give the same sum.
        var otherShapes = new[] { 4, 5, 6, 7, 8 };

        for (int startPrefix = 10; startPrefix < 100; startPrefix++) {
            foreach (int startValue in buckets[3, startPrefix]) {
                int suffix = startValue % 100;
                var used = new bool[9];
                used[3] = true;
                int result = Search(startValue, suffix, startPrefix, used, new int[6] { startValue, 0, 0, 0, 0, 0 }, 1, otherShapes, buckets);
                if (result > 0) return result;
            }
        }
        return -1;
    }

    private static int Search(int firstValue, int currentSuffix, int finalPrefix, bool[] used, int[] chain, int depth, int[] shapes, List<int>[,] buckets) {
        if (depth == 6) {
            if (currentSuffix != finalPrefix) return 0;
            int sum = 0;
            foreach (int v in chain) sum += v;
            return sum;
        }

        foreach (int s in shapes) {
            if (used[s]) continue;
            foreach (int v in buckets[s, currentSuffix]) {
                used[s] = true;
                chain[depth] = v;
                int r = Search(firstValue, v % 100, finalPrefix, used, chain, depth + 1, shapes, buckets);
                if (r > 0) return r;
                used[s] = false;
            }
        }
        return 0;
    }

    private static IEnumerable<int> Figurates(int sides) {
        // P_s(n) = n * ((s-2)*n - (s-4)) / 2
        int a = sides - 2, b = sides - 4;
        for (int n = 1; ; n++) {
            int v = n * (a * n - b) / 2;
            if (v < 1000) continue;
            if (v > 9999) yield break;
            yield return v;
        }
    }
}
