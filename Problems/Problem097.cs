namespace Project_Euler.Problems;

public class Problem097 : Problem {
    public override object Solve() {
        return LastTenDigits();
    }

    // Modular exponentiation: (28433 * 2^7830457 + 1) mod 10^10.
    private long LastTenDigits() {
        const long mod = 10_000_000_000L;
        long p = ModPow2(7_830_457, mod);
        // 28433 * p fits in long * long — need careful multiplication mod 10^10.
        long result = MulMod(28433, p, mod);
        result = (result + 1) % mod;
        return result;
    }

    private static long ModPow2(long exp, long mod) {
        long result = 1;
        long baseVal = 2;
        while (exp > 0) {
            if ((exp & 1) == 1) result = MulMod(result, baseVal, mod);
            exp >>= 1;
            baseVal = MulMod(baseVal, baseVal, mod);
        }
        return result;
    }

    private static long MulMod(long a, long b, long mod) {
        // a, b < 10^10, a*b can reach 10^20 — exceeds long. Split via 2^17 shift.
        long hi = a >> 17, lo = a & 0x1FFFF;
        return ((hi * b % mod) * 131072 % mod + lo * b % mod) % mod;
    }
}
