namespace Project_Euler.Problems;

public class Problem003 : Problem {
    public override object Solve() {
        return LargestPrimeFactor(600851475143);
    }

    private long LargestPrimeFactor(long n) {
        long largestPrimeFactor = 0;

        while ((n & 1) == 0) {
            largestPrimeFactor = 2;
            n >>= 1;
        }

        while (n % 3 == 0) {
            largestPrimeFactor = 3;
            n /= 3;
        }

        for (int i = 5; (long)i * i <= n; i += 6) {
            while (n % i == 0) {
                largestPrimeFactor = i;
                n /= i;
            }

            while (n % (i + 2) == 0) {
                largestPrimeFactor = i + 2;
                n /= i + 2;
            }
        }

        if (n > 4) largestPrimeFactor = n;
        return largestPrimeFactor;
    }
}