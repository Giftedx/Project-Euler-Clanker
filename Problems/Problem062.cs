namespace Project_Euler.Problems;

public class Problem062 : Problem {
    public override object Solve() {
        return SmallestCubeFivePermutations();
    }

    private long SmallestCubeFivePermutations() {
        // Group cubes by sorted-digit signature; stop at the first signature that reaches 5,
        // but only once all cubes of that digit length have been enumerated (since signatures
        // are bounded by digit count — within a length, equal-signature cubes are permutations).
        var groups = new Dictionary<string, List<long>>();
        int lastLen = 0;

        for (long n = 1; ; n++) {
            long cube = n * n * n;
            int len = DigitCount(cube);

            if (len != lastLen && lastLen != 0) {
                foreach (var kv in groups)
                    if (kv.Value.Count >= 5)
                        return kv.Value.Min();
                groups.Clear();
            }
            lastLen = len;

            string sig = DigitSignature(cube);
            if (!groups.TryGetValue(sig, out var list)) {
                list = new List<long>();
                groups[sig] = list;
            }
            list.Add(cube);
        }
    }

    private static string DigitSignature(long n) {
        Span<char> buf = stackalloc char[20];
        int i = 0;
        while (n > 0) { buf[i++] = (char)('0' + n % 10); n /= 10; }
        // insertion sort for a small span — avoids Array.Sort alloc
        for (int j = 1; j < i; j++) {
            char c = buf[j];
            int k = j - 1;
            while (k >= 0 && buf[k] > c) { buf[k + 1] = buf[k]; k--; }
            buf[k + 1] = c;
        }
        return new string(buf[..i]);
    }

    private static int DigitCount(long n) {
        int c = 0;
        while (n > 0) { c++; n /= 10; }
        return c;
    }
}
