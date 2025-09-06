namespace Project_Euler.Problems;

public class Problem029 : Problem {
    public override object Solve() {
        return DistinctPowers();
    }

    private long DistinctPowers() {
        long result = 0;

        const int noSquare = 81 * 99;
        const int noThird = 4 * (99 + 50);

        int thirdPowers = 0;
        int fourthPowers = 0;
        int fifthPowers = 0;
        int sixthPowers = 0;
        for (int i = 2; i <= 100; i++) {
            int n = i * 3;
            if (n > 100 &&
                (n > 200 || n % 2 != 0))
                thirdPowers++;

            n = i * 4;
            if (n > 200 &&
                (n > 300 || n % 3 != 0))
                fourthPowers++;

            n = i * 5;
            if (n > 100 &&
                (n > 200 || n % 2 != 0) &&
                (n > 300 || n % 3 != 0) &&
                (n > 400 || n % 4 != 0))
                fifthPowers++;

            n = i * 6;
            if (n > 100 &&
                (n > 200 || n % 2 != 0) &&
                (n > 300 || n % 3 != 0) &&
                (n > 400 || n % 4 != 0) &&
                (n > 500 || n % 5 != 0))
                sixthPowers++;
        }

        result += 99 + 50 + thirdPowers + fourthPowers + noSquare + noThird;
        result += 99 + 50 + thirdPowers + fourthPowers + fifthPowers + sixthPowers;

        return result;
    }
}