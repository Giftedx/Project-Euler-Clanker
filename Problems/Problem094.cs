namespace Project_Euler.Problems;

public class Problem094 : Problem {
    public override object Solve() {
        return AlmostEquilateralPerimeterSum(1_000_000_000L);
    }

    // Heron area of (a,a,b) with b=a±1 integer iff u^2 - 3v^2 = 4 where u = 3a ∓ 1.
    // Pell-like recurrence: (u, v) -> (2u + 3v, u + 2v) starting from (2, 0).
    // u mod 3 == 2  => a = (u+1)/3, b = a+1
    // u mod 3 == 1  => a = (u-1)/3, b = a-1
    private long AlmostEquilateralPerimeterSum(long maxPerim) {
        long sum = 0;
        long u = 2, v = 0;
        while (true) {
            (u, v) = (2 * u + 3 * v, u + 2 * v);
            long a;
            int sign;
            if (u % 3 == 2) { a = (u + 1) / 3; sign = +1; }
            else if (u % 3 == 1) { a = (u - 1) / 3; sign = -1; }
            else continue;
            if (a <= 1) continue;
            long b = a + sign;
            long perim = 2 * a + b;
            if (perim > maxPerim) break;
            sum += perim;
        }
        return sum;
    }
}
