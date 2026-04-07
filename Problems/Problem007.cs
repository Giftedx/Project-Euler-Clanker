namespace Project_Euler.Problems;

public class Problem007 : Problem {
    public override object Solve() {
        return NthPrime(10001);
    }

    private static int NthPrime(int n) {
        // Upper bound for the nth prime: p(n) < n * (ln(n) + ln(ln(n))) for n >= 6
        int limit = n < 6 ? 15 : (int)(n * (Math.Log(n) + Math.Log(Math.Log(n)) + 2));
        bool[] sieve = Library.GetSieve(limit);

        int count = 0;
        for (int i = 2; i < limit; i++) {
            if (sieve[i] && ++count == n) return i;
        }
        return -1;
    }
}
