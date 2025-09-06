namespace Project_Euler.Problems;

public class Problem033 : Problem {
    public override object Solve() {
        return DigitCancellingFractions();
    }

    private int DigitCancellingFractions() {
        int num = 1, den = 1;
        for (int d = 10; d < 100; d++)
        for (int n = 10; n < d; n++) {
            int n0 = n % 10, n1 = n / 10;
            int d0 = d % 10, d1 = d / 10;
            if ((n1 != d0 || n0 * d != n * d1) &&
                (n0 != d1 || n1 * d != n * d0)) continue;
            num *= n;
            den *= d;
        }

        return den / Library.Gcd(num, den);
    }
}