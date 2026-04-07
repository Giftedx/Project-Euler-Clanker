namespace Project_Euler.Problems;

public class Problem043 : Problem {
    private readonly int[] _tests = [2, 3, 5, 7, 11, 13, 17];
    private readonly bool[] _used = new bool[10];

    public override object Solve() {
        return SubStringDivisiblePandigitalSum();
    }

    private long SubStringDivisiblePandigitalSum() {
        long total = 0;
        BuildPandigitalSum(0, 0, ref total);
        return total;
    }

    private void BuildPandigitalSum(int depth, long number, ref long total) {
        if (depth is >= 4 and <= 10) {
            // Check last 3 digits divisibility
            int lastThree = (int)(number % 1000);
            if (lastThree % _tests[depth - 4] != 0)
                return;
        }

        if (depth == 10) {
            total += number;
            return;
        }

        int start = depth == 0 ? 1 : 0;
        for (int i = start; i <= 9; i++) {
            if (_used[i]) continue;
            _used[i] = true;
            BuildPandigitalSum(depth + 1, number * 10 + i, ref total);
            _used[i] = false;
        }
    }
}
