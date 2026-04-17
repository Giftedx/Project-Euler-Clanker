namespace Project_Euler.Problems;

public class Problem118 : Problem {
    public override object Solve() {
        return CountPandigitalPrimeSets();
    }

    // Enumerate subsets S of {1..9} by size 1..9; for each, find # of primes whose digits form exactly S.
    // Answer = sum over partitions of {1..9} of product of primeCount(S_i).
    // Iterate partitions via DP over ordered subsets: for each remaining mask, choose a subset containing the lowest unset bit.
    private int CountPandigitalPrimeSets() {
        int[] primesBySubset = new int[1 << 9];
        for (int mask = 1; mask < (1 << 9); mask++)
            primesBySubset[mask] = CountPrimesWithDigitMask(mask);

        var cache = new Dictionary<int, long>();
        long Count(int mask) {
            if (mask == 0) return 1;
            if (cache.TryGetValue(mask, out long v)) return v;
            int lowBit = mask & -mask;
            long total = 0;
            // Enumerate sub-subsets of mask that contain lowBit
            int rest = mask ^ lowBit;
            int sub = rest;
            while (true) {
                int chosen = sub | lowBit;
                total += (long)primesBySubset[chosen] * Count(mask ^ chosen);
                if (sub == 0) break;
                sub = (sub - 1) & rest;
            }
            cache[mask] = total;
            return total;
        }
        return (int)Count((1 << 9) - 1);
    }

    private static int CountPrimesWithDigitMask(int mask) {
        // Collect digits
        int[] digits = new int[System.Numerics.BitOperations.PopCount((uint)mask)];
        int idx = 0;
        for (int d = 1; d <= 9; d++) if ((mask & (1 << (d - 1))) != 0) digits[idx++] = d;
        Array.Sort(digits);

        int count = 0;
        do {
            long v = 0;
            foreach (int d in digits) v = v * 10 + d;
            if (IsPrime(v)) count++;
        } while (NextPermutation(digits));
        return count;
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

    private static bool IsPrime(long n) {
        if (n < 2) return false;
        if (n < 4) return true;
        if ((n & 1) == 0) return false;
        if (n % 3 == 0) return n == 3;
        long limit = (long)Math.Sqrt(n);
        for (long i = 5; i <= limit; i += 6) {
            if (n % i == 0 || n % (i + 2) == 0) return false;
        }
        return true;
    }
}
