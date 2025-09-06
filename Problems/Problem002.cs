namespace Project_Euler.Problems;

public class Problem002 : Problem {
    private const int Limit = 4000000;

    public override object Solve() {
        return EvenFibSum();
    }

    private int EvenFibSum() {
        int fib1 = 2;
        int fib2 = 8;
        int sum = fib1 + fib2;
        while (true) {
            int nextFib = 4 * fib2 + fib1;
            if (nextFib >= Limit) break;
            sum += nextFib;
            (fib1, fib2) = (fib2, nextFib);
        }

        return sum;
    }
}