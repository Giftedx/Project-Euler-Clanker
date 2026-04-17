namespace Project_Euler.Problems;

public class Problem087 : Problem {
    public override object Solve() {
        return CountPrimePowerTriples(50_000_000);
    }

    private int CountPrimePowerTriples(int limit) {
        int pMax2 = (int)Math.Sqrt(limit) + 1;
        int pMax3 = (int)Math.Cbrt(limit) + 1;
        int pMax4 = (int)Math.Pow(limit, 0.25) + 1;
        bool[] sieve = Library.GetSieve(pMax2 + 1);

        var primes = new List<int>();
        for (int i = 2; i <= pMax2; i++) if (sieve[i]) primes.Add(i);

        var seen = new HashSet<int>();
        foreach (int p2 in primes) {
            long sq = (long)p2 * p2;
            if (sq >= limit) break;
            foreach (int p3 in primes) {
                if (p3 > pMax3) break;
                long cb = (long)p3 * p3 * p3;
                if (sq + cb >= limit) break;
                foreach (int p4 in primes) {
                    if (p4 > pMax4) break;
                    long q = (long)p4 * p4; q *= q;
                    long total = sq + cb + q;
                    if (total >= limit) break;
                    seen.Add((int)total);
                }
            }
        }
        return seen.Count;
    }
}
