namespace Project_Euler.Problems;

public class Problem051 : Problem {
    private const int Limit = 1000000;

    private static readonly int[] Pow10 = [1, 10, 100, 1000, 10000, 100000, 1000000];

    public override object Solve() {
        return EightPrimeFamily();
    }

    private int EightPrimeFamily() {
        bool[] isPrime = Library.GetSieve(Limit);

        // Only masks with exactly 3 set bits (divisibility-by-3 argument)
        // Exclude bit 0 (units digit): replacing with even gives even → at most 5 primes
        // Positions 1-5 correspond to tens through hundred-thousands digit
        var masks = new List<int>();
        for (int mask = 1; mask < (1 << 5); mask++) {
            if (int.PopCount(mask) == 3)
                masks.Add(mask << 1); // shift left 1 to skip bit 0 (units)
        }

        int lowerBound = Limit / 10;

        foreach (int mask in masks) {
            // For each template: fix the non-masked positions, try all 10 replacements
            // Iterate over all possible values of the 3 fixed positions (units + 2 others)
            // This is more efficient than iterating all primes

            // Identify which positions are fixed (not in mask)
            int[] fixedPos = new int[3];
            int fi = 0;
            for (int bit = 0; bit < 6; bit++) {
                if ((mask & (1 << bit)) == 0)
                    fixedPos[fi++] = bit;
            }

            // Iterate all combinations of fixed digits
            for (int d0 = 1; d0 <= 9; d0 += 2) { // units digit must be odd (and not 5)
                if (d0 == 5) continue;
                for (int d1 = 0; d1 <= 9; d1++) {
                    for (int d2 = 0; d2 <= 9; d2++) {
                        int baseNum = d0 * Pow10[fixedPos[0]]
                                    + d1 * Pow10[fixedPos[1]]
                                    + d2 * Pow10[fixedPos[2]];

                        int count = 0;
                        int min = Limit;

                        for (int rep = 0; rep <= 9; rep++) {
                            int num = baseNum;
                            for (int bit = 0; bit < 6; bit++) {
                                if ((mask & (1 << bit)) != 0)
                                    num += rep * Pow10[bit];
                            }

                            if (num < lowerBound) continue; // not 6-digit
                            if (num >= Limit) continue;

                            if (isPrime[num]) {
                                count++;
                                if (num < min) min = num;
                            } else if (count + (9 - rep) < 8) {
                                break; // can't reach 8
                            }
                        }

                        if (count >= 8) return min;
                    }
                }
            }
        }

        return 0;
    }
}
