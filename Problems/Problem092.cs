namespace Project_Euler.Problems;

public class Problem092 : Problem {
    public override object Solve() {
        return CountArrivingAt89(10_000_000);
    }

    // Any chain converges to 1 or 89. Precompute the outcome of every digit-square-sum up to 567
    // (max for a 7-digit number: 7 * 81), then count n whose sum lands in the 89 basin.
    private int CountArrivingAt89(int limit) {
        const int maxSum = 9 * 9 * 7 + 1;
        int[] endsIn = new int[maxSum];
        endsIn[1] = 1;
        endsIn[89] = 89;
        for (int i = 2; i < maxSum; i++) {
            int n = i;
            while (endsIn[n] == 0) n = DigitSquareSum(n);
            endsIn[i] = endsIn[n];
        }

        int count = 0;
        for (int n = 1; n < limit; n++)
            if (endsIn[DigitSquareSum(n)] == 89)
                count++;
        return count;
    }

    private static int DigitSquareSum(int n) {
        int s = 0;
        while (n > 0) { int d = n % 10; s += d * d; n /= 10; }
        return s;
    }
}
