using static Project_Euler.Library;

namespace Project_Euler.Problems;

public class Problem035 : Problem {
    public override object Solve() {
        return CircularPrimeCount();
    }

    private int CircularPrimeCount() {
        int count = 4;

        for (int a = 1; a <= 9; a += 2) {
            if (a == 5) continue;
            for (int b = a; b <= 9; b += 2) {
                if (b == 5) continue;
                if (IsPrime(a * 10 + b) && IsPrime(b * 10 + a))
                    count += a != b ? 2 : 1;
                int startC = a == b ? a : a + 2;
                for (int c = startC; c <= 9; c += 2) {
                    if (c == 5) continue;
                    if (IsPrime(ComposeNumber(a, b, c)) &&
                        IsPrime(ComposeNumber(b, c, a)) &&
                        IsPrime(ComposeNumber(c, a, b)))
                        count += a != c ? 3 : 1;

                    int startD = a == b && a == c ? a : a + 2;
                    for (int d = startD; d <= 9; d += 2) {
                        if (d == 5) continue;
                        if (IsPrime(ComposeNumber(a, b, c, d)) &&
                            IsPrime(ComposeNumber(b, c, d, a)) &&
                            IsPrime(ComposeNumber(c, d, a, b)) &&
                            IsPrime(ComposeNumber(d, a, b, c)))
                            count += 4;

                        int startE = a == b && a == c && a == d ? a : a + 2;
                        for (int e = startE; e <= 9; e += 2) {
                            if (e == 5) continue;
                            if (IsPrime(ComposeNumber(a, b, c, d, e)) &&
                                IsPrime(ComposeNumber(b, c, d, e, a)) &&
                                IsPrime(ComposeNumber(c, d, e, a, b)) &&
                                IsPrime(ComposeNumber(d, e, a, b, c)) &&
                                IsPrime(ComposeNumber(e, a, b, c, d)))
                                count += a != e ? 5 : 1;

                            for (int f = a + 2; f <= 9; f += 2) {
                                if (f == 5) continue;
                                if (IsPrime(ComposeNumber(a, b, c, d, e, f)) &&
                                    IsPrime(ComposeNumber(b, c, d, e, f, a)) &&
                                    IsPrime(ComposeNumber(c, d, e, f, a, b)) &&
                                    IsPrime(ComposeNumber(d, e, f, a, b, c)) &&
                                    IsPrime(ComposeNumber(e, f, a, b, c, d)) &&
                                    IsPrime(ComposeNumber(f, a, b, c, d, e)))
                                    count += 6;
                            }
                        }
                    }
                }
            }
        }

        return count;
    }

    private int ComposeNumber(params int[] digits) {
        int result = 0;
        foreach (int t in digits) result = result * 10 + t;
        return result;
    }
}