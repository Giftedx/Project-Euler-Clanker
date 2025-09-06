namespace Project_Euler.Problems;

public class Problem005 : Problem {
    public override object Solve() {
        return MinimumEvenlyDivisibleByRange(1, 20);
    }

    private string MinimumEvenlyDivisibleByRange(int min, int max) {
        ulong result = 1;
        for (int i = min; i <= max; i++) result = LeastCommonMultiple((ulong)i, result);
        return result.ToString();
    }

    private ulong LeastCommonMultiple(ulong a, ulong b) {
        return a / GreatestCommonDivisor(a, b) * b;
    }

    private ulong GreatestCommonDivisor(ulong a, ulong b) {
        while (true) {
            if (a == 0) return b;
            ulong a1 = a;
            a = b % a;
            b = a1;
        }
    }
}