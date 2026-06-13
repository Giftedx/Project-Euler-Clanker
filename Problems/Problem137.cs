namespace Project_Euler.Problems;

public class Problem137 : Problem {
    public override object Solve() {
        return NthGoldenNugget(15);
    }

    // AF(x) = sum F_i x^i = x / (1 - x - x^2). AF(x) = N (integer) iff x = (-1 + sqrt(5N^2+2N+1))/(2N+2)... rearranging:
    // N = x/(1-x-x^2). For N to be a positive integer, 5N^2 + 2N + 1 must be a perfect square.
    // Equivalently, related Pell equation: 5N^2 + 2N + 1 = k^2, solutions via Fibonacci-like recurrence.
    // Known: nth golden nugget is F_{2n} * F_{2n+1} (Fibonacci products).
    private long NthGoldenNugget(int n) {
        long[] f = new long[2 * n + 3];
        f[1] = 1; f[2] = 1;
        for (int i = 3; i < f.Length; i++) f[i] = f[i - 1] + f[i - 2];
        return f[2 * n] * f[2 * n + 1];
    }
}
