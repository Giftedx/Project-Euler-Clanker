namespace Project_Euler.Problems;

public class Problem090 : Problem {
    public override object Solve() {
        return CountCubeDigitArrangements();
    }

    // Each cube holds 6 faces chosen from {0..9}. Represent as a 10-bit mask.
    // Treat 6/9 as interchangeable: if a cube has 6 or 9, it implicitly has both.
    // A pair of masks (A,B) is valid if every required two-digit square {01,04,09,16,25,36,49,64,81}
    // can be displayed by one face of A and one of B (or vice versa).
    private int CountCubeDigitArrangements() {
        var cubes = new List<int>();
        EnumerateCubes(cubes, 0, 0, 0);

        (int a, int b)[] squares = { (0, 1), (0, 4), (0, 9), (1, 6), (2, 5), (3, 6), (4, 9), (6, 4), (8, 1) };
        int count = 0;
        for (int i = 0; i < cubes.Count; i++) {
            int A = Normalize69(cubes[i]);
            for (int j = i; j < cubes.Count; j++) {
                int B = Normalize69(cubes[j]);
                if (AllSquaresRepresentable(A, B, squares)) count++;
            }
        }
        return count;
    }

    private static int Normalize69(int mask) {
        if ((mask & (1 << 6)) != 0) mask |= 1 << 9;
        if ((mask & (1 << 9)) != 0) mask |= 1 << 6;
        return mask;
    }

    private static bool AllSquaresRepresentable(int A, int B, (int a, int b)[] squares) {
        foreach (var (a, b) in squares) {
            bool ok = ((A >> a) & 1) == 1 && ((B >> b) & 1) == 1
                   || ((A >> b) & 1) == 1 && ((B >> a) & 1) == 1;
            if (!ok) return false;
        }
        return true;
    }

    private static void EnumerateCubes(List<int> cubes, int start, int chosen, int mask) {
        if (chosen == 6) { cubes.Add(mask); return; }
        for (int d = start; d <= 9; d++) {
            if (9 - d < 6 - chosen - 1) break; // not enough digits left
            EnumerateCubes(cubes, d + 1, chosen + 1, mask | (1 << d));
        }
    }
}
