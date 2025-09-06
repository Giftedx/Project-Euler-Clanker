namespace Project_Euler.Problems;

public class Problem043 : Problem {
    private readonly int[] _tests = [2, 3, 5, 7, 11, 13, 17];
    private readonly bool[] _used = new bool[10];

    public override object Solve() {
        return SubStringDivisiblePandigitalSum();
    }

    private long SubStringDivisiblePandigitalSum() {
        long total = 0;
        char[] buffer = new char[10];
        BuildPandigitalSum(0, buffer, ref total);
        return total;
    }

    private void BuildPandigitalSum(int depth, char[] buffer, ref long total) {
        if (depth is >= 4 and <= 10) {
            int value = (buffer[depth - 3] & 15) * 100 +
                        (buffer[depth - 2] & 15) * 10 +
                        (buffer[depth - 1] & 15);
            if (value % _tests[depth - 4] != 0)
                return;
        }

        if (depth == 10) {
            total += long.Parse(new string(buffer));
            return;
        }

        for (char i = depth == 0 ? '1' : '0'; i <= '9'; i++) {
            int index = i - '0';
            if (_used[index]) continue;

            _used[index] = true;
            buffer[depth] = i;
            BuildPandigitalSum(depth + 1, buffer, ref total);
            _used[index] = false;
        }
    }
}