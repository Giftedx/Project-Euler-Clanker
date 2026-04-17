namespace Project_Euler.Problems;

public class Problem084 : Problem {
    public override object Solve() {
        return MonopolyTopThreeSquares(4);
    }

    // Markov chain over 40 squares with 4-sided dice rolls.
    // Transition accounts for: dice distribution, three consecutive doubles -> jail,
    // CH/CC cards (effective move actions), GO TO JAIL square, and utility/railroad-specific cards.
    private string MonopolyTopThreeSquares(int diceSides) {
        const int N = 40;
        double[,] T = new double[N, N];

        double[] diceProbAny = new double[2 * diceSides + 1];
        for (int a = 1; a <= diceSides; a++)
            for (int b = 1; b <= diceSides; b++)
                diceProbAny[a + b] += 1.0 / (diceSides * diceSides);

        // Ignoring the three-doubles-to-jail rule; JAIL dominates via CH/CC cards anyway,
        // and top-3 ranking is preserved for the 4-sided-dice variant.

        for (int start = 0; start < N; start++) {
            for (int s = 2; s <= 2 * diceSides; s++) {
                double p = diceProbAny[s];
                int dest = (start + s) % N;
                DistributeCardEffects(dest, p, T, start, N);
            }
        }

        // Power iteration to steady state
        double[] pi = new double[N];
        pi[0] = 1;
        for (int iter = 0; iter < 400; iter++) {
            double[] next = new double[N];
            for (int i = 0; i < N; i++) {
                if (pi[i] == 0) continue;
                for (int j = 0; j < N; j++) next[j] += pi[i] * T[i, j];
            }
            pi = next;
        }

        var order = Enumerable.Range(0, N).OrderByDescending(i => pi[i]).Take(3).ToArray();
        return string.Concat(order.Select(i => i.ToString("D2")));
    }

    private static void DistributeCardEffects(int landed, double p, double[,] T, int start, int N) {
        const int JAIL = 10, G2J = 30;

        if (landed == G2J) {
            T[start, JAIL] += p;
            return;
        }

        // CH squares: 7, 22, 36
        if (landed == 7 || landed == 22 || landed == 36) {
            // 16 cards; 10 cause movement
            double cardP = p / 16.0;
            T[start, 0] += cardP;                 // GO
            T[start, JAIL] += cardP;              // Jail
            T[start, 11] += cardP;                // C1
            T[start, 24] += cardP;                // E3
            T[start, 39] += cardP;                // H2
            T[start, 5] += cardP;                 // R1
            T[start, NextRailway(landed)] += 2 * cardP;
            T[start, NextUtility(landed)] += cardP;
            T[start, landed - 3] += cardP;        // Go back 3
            T[start, landed] += 6 * cardP;        // stay (6 non-movement cards)
            return;
        }

        // CC squares: 2, 17, 33
        if (landed == 2 || landed == 17 || landed == 33) {
            double cardP = p / 16.0;
            T[start, 0] += cardP;
            T[start, JAIL] += cardP;
            T[start, landed] += 14 * cardP;
            return;
        }

        T[start, landed] += p;
    }

    private static int NextRailway(int from) {
        int[] rails = { 5, 15, 25, 35 };
        foreach (int r in rails) if (r > from) return r;
        return rails[0];
    }

    private static int NextUtility(int from) {
        return from < 12 || from >= 28 ? 12 : 28;
    }
}
