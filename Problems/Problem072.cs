namespace Project_Euler.Problems;

public class Problem072 : Problem {
    public override object Solve() {
        return SumTotients(1_000_000);
    }

    // Count reduced proper fractions n/d with d in [2, N]: sum of phi(d) for d=2..N.
    // Linear totient sieve avoids per-n factorization.
    private long SumTotients(int limit) {
        int[] phi = new int[limit + 1];
        for (int i = 0; i <= limit; i++) phi[i] = i;
        for (int i = 2; i <= limit; i++) {
            if (phi[i] != i) continue; // i is composite — phi[i] was updated by a prime factor
            for (int j = i; j <= limit; j += i)
                phi[j] -= phi[j] / i;
        }

        long sum = 0;
        for (int i = 2; i <= limit; i++) sum += phi[i];
        return sum;
    }
}
