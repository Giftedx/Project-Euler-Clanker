namespace Project_Euler.Problems;

public class Problem093 : Problem {
    public override object Solve() {
        return BestDigitSet();
    }

    // For each 4-subset of digits, enumerate all expressions (24 permutations * 64 op combos
    // * 5 parenthesizations). Track consecutive integer run from 1. Return abcd of best set.
    private int BestDigitSet() {
        int bestRun = 0;
        int bestCode = 0;

        for (int a = 1; a <= 6; a++)
            for (int b = a + 1; b <= 7; b++)
                for (int c = b + 1; c <= 8; c++)
                    for (int d = c + 1; d <= 9; d++) {
                        int run = ConsecutiveRun(new[] { a, b, c, d });
                        if (run <= bestRun) continue;
                        bestRun = run;
                        bestCode = a * 1000 + b * 100 + c * 10 + d;
                    }
        return bestCode;
    }

    private static int ConsecutiveRun(int[] digits) {
        var reachable = new HashSet<int>();
        int[] perm = (int[])digits.Clone();
        Array.Sort(perm);
        do {
            double a = perm[0], b = perm[1], c = perm[2], d = perm[3];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    for (int k = 0; k < 4; k++)
                        for (int p = 0; p < 5; p++) {
                            double? r = Evaluate(a, b, c, d, i, j, k, p);
                            if (r.HasValue && r.Value > 0 && Math.Abs(r.Value - Math.Round(r.Value)) < 1e-9)
                                reachable.Add((int)Math.Round(r.Value));
                        }
        } while (NextPermutation(perm));

        int run = 0;
        while (reachable.Contains(run + 1)) run++;
        return run;
    }

    private static double? Evaluate(double a, double b, double c, double d, int op1, int op2, int op3, int parens) {
        // Parenthesization modes:
        // 0: ((a op b) op c) op d
        // 1: (a op (b op c)) op d
        // 2: a op ((b op c) op d)
        // 3: a op (b op (c op d))
        // 4: (a op b) op (c op d)
        try {
            return parens switch {
                0 => Apply(Apply(Apply(a, b, op1), c, op2), d, op3),
                1 => Apply(Apply(a, Apply(b, c, op2), op1), d, op3),
                2 => Apply(a, Apply(Apply(b, c, op2), d, op3), op1),
                3 => Apply(a, Apply(b, Apply(c, d, op3), op2), op1),
                4 => Apply(Apply(a, b, op1), Apply(c, d, op3), op2),
                _ => null
            };
        } catch { return null; }
    }

    private static double Apply(double x, double y, int op) {
        return op switch {
            0 => x + y,
            1 => x - y,
            2 => x * y,
            3 => y == 0 ? throw new DivideByZeroException() : x / y,
            _ => 0
        };
    }

    private static bool NextPermutation(int[] arr) {
        int i = arr.Length - 1;
        while (i > 0 && arr[i - 1] >= arr[i]) i--;
        if (i <= 0) return false;
        int j = arr.Length - 1;
        while (arr[j] <= arr[i - 1]) j--;
        (arr[i - 1], arr[j]) = (arr[j], arr[i - 1]);
        for (int k = arr.Length - 1; i < k; i++, k--)
            (arr[i], arr[k]) = (arr[k], arr[i]);
        return true;
    }
}
