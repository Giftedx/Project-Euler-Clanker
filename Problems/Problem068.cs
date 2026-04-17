namespace Project_Euler.Problems;

public class Problem068 : Problem {
    public override object Solve() {
        return MaximumMagic5GonRing();
    }

    // 5-gon ring layout: outer = a,b,c,d,e  inner = f,g,h,i,j
    // Five groups (each a line): a+f+g, b+g+h, c+h+i, d+i+j, e+j+f — all equal S.
    // Each outer appears in 1 line, each inner in 2; so 10 must be OUTER to hit 16 digits
    // (inner 10 would contribute two extra digits, giving 17).
    private long MaximumMagic5GonRing() {
        long best = 0;
        int[] nums = new int[10];
        for (int i = 0; i < 10; i++) nums[i] = i + 1;

        do {
            int a = nums[0], b = nums[1], c = nums[2], d = nums[3], e = nums[4];
            int f = nums[5], g = nums[6], h = nums[7], i = nums[8], j = nums[9];

            if (a == 10 || b == 10 || c == 10 || d == 10 || e == 10) {
                int s = a + f + g;
                if (b + g + h != s) continue;
                if (c + h + i != s) continue;
                if (d + i + j != s) continue;
                if (e + j + f != s) continue;
                // require 'a' to be the min of outers so each ring is counted once
                if (a > b || a > c || a > d || a > e) continue;

                long val = Concat(a, f, g);
                val = Concat(val, b, g, h);
                val = Concat(val, c, h, i);
                val = Concat(val, d, i, j);
                val = Concat(val, e, j, f);

                if (val > best) best = val;
            }
        } while (NextPermutation(nums));

        return best;
    }

    private static long Concat(int x, int y, int z) {
        return AppendNumber(AppendNumber(x, y), z);
    }

    private static long Concat(long acc, int x, int y, int z) {
        return AppendNumber(AppendNumber(AppendNumber(acc, x), y), z);
    }

    private static long AppendNumber(long acc, int n) {
        return n < 10 ? acc * 10 + n : acc * 100 + n;
    }

    private static bool NextPermutation(int[] arr) {
        int i = arr.Length - 1;
        while (i > 0 && arr[i - 1] >= arr[i]) i--;
        if (i <= 0) return false;
        int j = arr.Length - 1;
        while (arr[j] <= arr[i - 1]) j--;
        (arr[i - 1], arr[j]) = (arr[j], arr[i - 1]);
        for (int k = arr.Length - 1; i < k; i++, k--)
            (arr[i], arr[k]) = (arr[k], arr[i]);
        return true;
    }
}
