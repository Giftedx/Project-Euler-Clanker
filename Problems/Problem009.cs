namespace Project_Euler.Problems;

public class Problem009 : Problem {
    public override object Solve() {
        return PythagoreanTripletProduct(1000);
    }

    private int PythagoreanTripletProduct(int n) {
        for (int a = 1; a < n / 3; a++)
        for (int b = a + 1; b < n / 2; b++) {
            int c = n - a - b;
            if (IsTriplet(a, b, c)) return a * b * c;
        }

        return 0;
    }

    private bool IsTriplet(int a, int b, int c) {
        return a * a + b * b == c * c;
    }
}