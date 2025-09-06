namespace Project_Euler.Problems;

public class Problem020 : Problem {
    public override object Solve() {
        return ExponentSum();
    }

    private int ExponentSum() {
        var result = Library.Factorial(100);
        return Library.SumDigits(result);
    }
}