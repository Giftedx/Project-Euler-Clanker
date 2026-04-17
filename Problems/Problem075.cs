namespace Project_Euler.Problems;

public class Problem075 : Problem {
    public override object Solve() {
        return CountUniquePerimeters(1_500_000);
    }

    // Euclid's formula: primitive Pythagorean triples (a,b,c) with a=m^2-n^2, b=2mn, c=m^2+n^2,
    // m>n>0, gcd(m,n)=1, m-n odd. Perimeter P = 2*m*(m+n). Each primitive P and its multiples k*P
    // map to wire lengths; count how many lengths have exactly one triangle.
    private int CountUniquePerimeters(int limit) {
        int[] counts = new int[limit + 1];
        int mMax = (int)Math.Sqrt(limit / 2) + 1;
        for (int m = 2; m <= mMax; m++) {
            for (int n = 1; n < m; n++) {
                if (((m ^ n) & 1) == 0) continue;
                if (Library.Gcd(m, n) != 1) continue;
                int p = 2 * m * (m + n);
                if (p > limit) break;
                for (int k = p; k <= limit; k += p) counts[k]++;
            }
        }
        int singular = 0;
        for (int i = 0; i <= limit; i++) if (counts[i] == 1) singular++;
        return singular;
    }
}
