namespace Project_Euler.Problems;

public class Problem112 : Problem {
    public override object Solve() {
        return FirstWhereBouncyFraction99();
    }

    // Iterate n, count bouncy ratio until it hits 99/100.
    private int FirstWhereBouncyFraction99() {
        int bouncy = 0;
        for (int n = 1; ; n++) {
            if (IsBouncy(n)) bouncy++;
            if (bouncy * 100 == 99 * n) return n;
        }
    }

    private static bool IsBouncy(int n) {
        bool inc = false, dec = false;
        int prev = n % 10;
        n /= 10;
        while (n > 0) {
            int d = n % 10;
            if (d < prev) inc = true;
            else if (d > prev) dec = true;
            if (inc && dec) return true;
            prev = d;
            n /= 10;
        }
        return false;
    }
}
