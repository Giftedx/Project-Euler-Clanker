namespace Project_Euler.Problems;

public class Problem115 : Problem {
    public override object Solve() {
        return FirstLengthOverMillion(50);
    }

    private long FirstLengthOverMillion(int minBlock) {
        long[] f = new long[1];
        f[0] = 1;
        int n = 0;
        while (true) {
            n++;
            long[] nf = new long[n + 1];
            Array.Copy(f, nf, f.Length);
            nf[n] = nf[n - 1];
            for (int k = minBlock; k <= n; k++) {
                nf[n] += n - k - 1 >= 0 ? nf[n - k - 1] : 1;
            }
            f = nf;
            if (f[n] > 1_000_000) return n;
        }
    }
}
