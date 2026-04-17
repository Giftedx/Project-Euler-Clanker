namespace Project_Euler.Problems;

public class Problem005 : Problem {
    public override object Solve() {
        return MinimumEvenlyDivisibleByRange(1, 20);
    }

    private int MinimumEvenlyDivisibleByRange(int min, int max) {
        int result = 1;
        for (int i = min; i <= max; i++)
            result = i / Library.Gcd(i, result) * result;
        return result;
    }
}
