namespace Project_Euler.Problems;

public class Problem069 : Problem {
    public override object Solve() {
        return TotientMaximum(1_000_000);
    }

    // n / phi(n) = product over distinct primes p | n of p / (p - 1).
    // This is maximized by taking the product of as many small distinct primes as possible.
    private int TotientMaximum(int limit) {
        int[] primes = { 2, 3, 5, 7, 11, 13, 17, 19, 23 };
        int product = 1;
        foreach (int p in primes) {
            long next = (long)product * p;
            if (next > limit) break;
            product = (int)next;
        }
        return product;
    }
}
