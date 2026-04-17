namespace Project_Euler.Problems;

public class Problem101 : Problem {
    public override object Solve() {
        return SumOfFITs();
    }

    // True generator: u_n = 1 - n + n^2 - n^3 + n^4 - n^5 + n^6 - n^7 + n^8 - n^9 + n^10.
    // For k in 1..10, fit polynomial of degree k-1 through (1, u_1), ..., (k, u_k) via Lagrange
    // and compute first incorrect term (FIT) — the next x where OP(k,x) != u_x.
    // Sum over k=1..10.
    private long SumOfFITs() {
        long True(long n) {
            long sum = 0;
            long sign = 1;
            long p = 1;
            for (int i = 0; i <= 10; i++) {
                sum += sign * p;
                sign = -sign;
                p *= n;
            }
            return sum;
        }

        long total = 0;
        for (int k = 1; k <= 10; k++) {
            long[] y = new long[k];
            for (int i = 0; i < k; i++) y[i] = True(i + 1);
            // Find first n where Lagrange interpolation != True(n)
            for (int n = 1; ; n++) {
                long lag = LagrangeAt(y, n);
                if (lag == True(n)) continue;
                total += lag;
                break;
            }
        }
        return total;
    }

    private static long LagrangeAt(long[] y, int x) {
        // Nodes are 1..k with k = y.Length. Use rational Lagrange then round.
        int k = y.Length;
        System.Numerics.BigInteger num = 0;
        System.Numerics.BigInteger den = 1;
        for (int i = 0; i < k; i++) {
            System.Numerics.BigInteger termNum = y[i];
            System.Numerics.BigInteger termDen = 1;
            for (int j = 0; j < k; j++) {
                if (i == j) continue;
                termNum *= x - (j + 1);
                termDen *= (i + 1) - (j + 1);
            }
            // Combine term to running sum with common denominator
            num = num * termDen + termNum * den;
            den *= termDen;
        }
        return (long)(num / den);
    }
}
