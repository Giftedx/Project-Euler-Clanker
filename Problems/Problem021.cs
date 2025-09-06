namespace Project_Euler.Problems;

public class Problem021 : Problem {
    public override object Solve() {
        return AmicableSumBelow(10000);
    }

    private int AmicableSumBelow(int n) {
        FillDivisors(n + 1, out int[] divisors);
        int sum = 0;
        for (int i = 1; i < n + 1; ++i) {
            int j = divisors[i];
            if (j > i && j <= n && divisors[j] == i)
                sum += i + j;
        }

        return sum;
    }

    private void FillDivisors(int n, out int[] divisors) {
        divisors = new int[n];
        for (int i = 1; i < n; ++i)
        for (int j = 2 * i; j <= n - 1; j += i)
            divisors[j] += i;
    }
}