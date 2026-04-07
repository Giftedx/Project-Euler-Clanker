namespace Project_Euler.Problems;

public class Problem048 : Problem {
    public override object Solve() {
        return SelfPowSumLastTen();
    }

    private static long SelfPowSumLastTen() {
        const long mod = 10_000_000_000L;
        long sum = 0;
        for (int i = 1; i <= 1000; i++) {
            sum = (sum + ModPow(i, i, mod)) % mod;
        }
        return sum;
    }

    private static long ModPow(long baseVal, long exp, long mod) {
        long result = 1;
        baseVal %= mod;
        while (exp > 0) {
            if ((exp & 1) == 1)
                result = (long)((decimal)result * baseVal % mod);
            exp >>= 1;
            baseVal = (long)((decimal)baseVal * baseVal % mod);
        }
        return result;
    }
}
