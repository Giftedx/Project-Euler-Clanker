namespace Project_Euler.Problems;

public class Problem123 : Problem {
    public override object Solve() {
        return FirstPrimeSquareRemainderOverLimit();
    }

    // r_n = ((p-1)^n + (p+1)^n) mod p^2. Expands: n even -> 2; n odd -> 2*n*p mod p^2.
    // Want r > 10^10, so need n odd and 2*n*p > 10^10, i.e., n > 5*10^9 / p.
    // Need n = index of prime. So find smallest odd n where 2*n*p_n > 10^10.
    private int FirstPrimeSquareRemainderOverLimit() {
        const long target = 10_000_000_000L;
        const int sieveLimit = 400_000;
        bool[] sieve = Library.GetSieve(sieveLimit);
        var primes = new List<int>();
        for (int i = 2; i < sieveLimit; i++) if (sieve[i]) primes.Add(i);

        for (int n = 1; n <= primes.Count; n++) {
            if ((n & 1) == 0) continue;
            long r = (2L * n * primes[n - 1]) % ((long)primes[n - 1] * primes[n - 1]);
            if (r > target) return n;
        }
        return -1;
    }
}
