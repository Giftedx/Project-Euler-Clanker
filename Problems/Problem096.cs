namespace Project_Euler.Problems;

public class Problem096 : Problem {
    public override object Solve() {
        return SumTopLeftThreeDigits();
    }

    // 50 puzzles, each 9 lines. Solve each with bitmask constraint propagation + backtracking.
    // After solving, read top-left 3 digits and accumulate.
    private int SumTopLeftThreeDigits() {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "p096_sudoku.txt");
        string[] lines = File.ReadAllLines(path);
        int total = 0;
        for (int i = 0; i < lines.Length; i += 10) {
            int[,] grid = new int[9, 9];
            for (int r = 0; r < 9; r++) {
                string row = lines[i + 1 + r];
                for (int c = 0; c < 9; c++)
                    grid[r, c] = row[c] - '0';
            }
            Solve(grid);
            total += grid[0, 0] * 100 + grid[0, 1] * 10 + grid[0, 2];
        }
        return total;
    }

    private static bool Solve(int[,] g) {
        int br = -1, bc = -1, bestOpts = 10;
        int[] rowMask = new int[9], colMask = new int[9], boxMask = new int[9];
        for (int r = 0; r < 9; r++)
            for (int c = 0; c < 9; c++) {
                int v = g[r, c];
                if (v == 0) continue;
                int bit = 1 << v;
                rowMask[r] |= bit; colMask[c] |= bit; boxMask[3 * (r / 3) + c / 3] |= bit;
            }

        for (int r = 0; r < 9; r++)
            for (int c = 0; c < 9; c++) {
                if (g[r, c] != 0) continue;
                int avail = ~(rowMask[r] | colMask[c] | boxMask[3 * (r / 3) + c / 3]) & 0x3FE;
                int cnt = System.Numerics.BitOperations.PopCount((uint)avail);
                if (cnt < bestOpts) { bestOpts = cnt; br = r; bc = c; }
            }

        if (br == -1) return true;
        if (bestOpts == 0) return false;

        int mask = ~(rowMask[br] | colMask[bc] | boxMask[3 * (br / 3) + bc / 3]) & 0x3FE;
        while (mask != 0) {
            int bit = mask & -mask;
            int v = System.Numerics.BitOperations.TrailingZeroCount(bit);
            g[br, bc] = v;
            if (Solve(g)) return true;
            g[br, bc] = 0;
            mask &= mask - 1;
        }
        return false;
    }
}
