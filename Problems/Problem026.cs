namespace Project_Euler.Problems;

public class Problem026 : Problem {
    public override object Solve() {
        return GetLongestCycleDenominator(1000);
    }

    private int GetLongestCycleDenominator(int limit) {
        int maxCycleLength = 0;
        int denominatorWithMaxCycle = 0;

        for (int d = limit - 1; d >= 2; d--) {
            if (maxCycleLength >= d) break;

            if (d % 2 == 0 || d % 5 == 0 || !Library.IsPrime(d)) continue;

            int cycleLength = GetCycleLength(d);
            if (cycleLength <= maxCycleLength) continue;
            maxCycleLength = cycleLength;
            denominatorWithMaxCycle = d;
        }

        return denominatorWithMaxCycle;
    }

    private int GetCycleLength(int d) {
        int remainder = 1;
        int position = 0;

        do {
            remainder = remainder * 10 % d;
            position++;
        } while (remainder != 1 && remainder != 0);

        return remainder == 0 ? 0 : position;
    }
}