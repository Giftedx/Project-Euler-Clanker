namespace Project_Euler.Problems;

public class Problem030 : Problem {
    public override object Solve() {
        return SumAllFifthPowers();
    }

    private long SumAllFifthPowers() {
        long[] powers = new long[10];
        long[] diffs = new long[9];
        int[] digits = new int[100];

        for (int i = 0; i < 10; i++) {
            powers[i] = 1;
            for (int j = 0; j < 5; j++) powers[i] *= i;
        }

        for (int i = 0; i < 9; i++) diffs[i] = powers[i + 1] - powers[i];

        long sum = 0, sumPow = 1, number = 1;
        int pos = 0;
        digits[pos] = 1;

        while (true) {
            int[] counts = new int[10];
            for (int i = 0; i <= pos; i++)
                counts[digits[i]]++;

            long k = sumPow;
            while (k > 0) {
                counts[(int)(k % 10)]--;
                k /= 10;
            }

            int check;
            for (check = 1; check < 10; check++)
                if (counts[check] != 0)
                    break;

            if (check == 10 && pos > 0) sum += sumPow;

            if (number * 10 + 9 <= sumPow + powers[9]) {
                int i = digits[pos++];
                digits[pos] = i;
                number = number * 10 + i;
                sumPow += powers[i];
            } else {
                while (pos > 0 && digits[pos] == 9) {
                    pos--;
                    number /= 10;
                    sumPow -= powers[9];
                }

                if (digits[pos] == 9) break;
                number++;
                sumPow += diffs[digits[pos]];
                digits[pos]++;
            }
        }

        return sum;
    }
}