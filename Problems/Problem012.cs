namespace Project_Euler.Problems;

public class Problem012 : Problem {
    public override object Solve() {
        return HighlyDivisibleTriangle(500);
    }

    private long HighlyDivisibleTriangle(int minDivisors) {
        int n = 1;
        while (true) {
            int divisorsCount;
            if (n % 2 == 0) divisorsCount = Tau(n >> 1) * Tau(n + 1);
            else divisorsCount = Tau(n) * Tau((n + 1) >> 1);

            if (divisorsCount > minDivisors)
                return ((long)n * (n + 1)) >> 1;

            n++;
        }
    }

    private int Tau(int num) {
        int count = 0;
        int root = (int)Math.Sqrt(num);
        for (int i = 1; i <= root; i++)
            if (num % i == 0)
                count += 2;
        if (root * root == num) count--;
        return count;
    }
}