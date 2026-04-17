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
                result = MulMod(result, baseVal, mod);
            exp >>= 1;
            baseVal = MulMod(baseVal, baseVal, mod);
        }
        return result;
    }

    private static long MulMod(long a, long b, long mod) {
        // Split to avoid overflow: a*b can reach 10^20, exceeding long/ulong
        long hi = a >> 17, lo = a & 0x1FFFF;
        return ((hi * b % mod) * 131072 % mod + lo * b % mod) % mod;
    }
}
