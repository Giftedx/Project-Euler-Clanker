namespace Project_Euler.Problems;

public class Problem043 : Problem {

    public override object Solve() {
        return SubStringDivisiblePandigitalSum();
    }

    private static long SubStringDivisiblePandigitalSum() {
        int[] divs = [17, 13, 11, 7, 5, 3, 2];

        // Start: 3-digit multiples of 17 with distinct digits
        var candidates = new List<(int mask, int left2, long num)>();
        for (int v = 17; v <= 999; v += 17) {
            int d0 = v / 100, d1 = (v / 10) % 10, d2 = v % 10;
            int mask = (1 << d0) | (1 << d1) | (1 << d2);
            if (int.PopCount(mask) != 3) continue;
            candidates.Add((mask, d0 * 10 + d1, v));
        }

        // Extend left one digit at a time
        long multiplier = 1000;
        for (int step = 1; step < 7; step++) {
            int div = divs[step];
            var next = new List<(int mask, int left2, long num)>();

            foreach (var (mask, left2, num) in candidates) {
                for (int d = 0; d <= 9; d++) {
                    if ((mask & (1 << d)) != 0) continue;
                    int window = d * 100 + left2;
                    if (window % div != 0) continue;
                    next.Add((mask | (1 << d), d * 10 + left2 / 10, d * multiplier + num));
                }
            }
            candidates = next;
            multiplier *= 10;
        }

        // Prepend the one remaining digit as d1
        long total = 0;
        foreach (var (mask, _, num) in candidates) {
            for (int d = 0; d <= 9; d++) {
                if ((mask & (1 << d)) != 0) continue;
                total += d * multiplier + num;
            }
        }
        return total;
    }
}
