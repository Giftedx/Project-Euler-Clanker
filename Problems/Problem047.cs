namespace Project_Euler.Problems;

public class Problem047 : Problem {
    public override object Solve() {
        return FirstInFourPrimeRun();
    }

    private int FirstInFourPrimeRun() {
        const int limit = 200000;
        byte[] factorCounts = new byte[limit];

        for (int i = 2; i < limit; i++) {
            if (factorCounts[i] != 0) continue;
            for (int j = i; j < limit; j += i)
                factorCounts[j]++;
        }

        for (int i = 2; i < limit - 3; i++)
            if (factorCounts[i] == 4 &&
                factorCounts[i + 1] == 4 &&
                factorCounts[i + 2] == 4 &&
                factorCounts[i + 3] == 4)
                return i;
        return -1;
    }
}