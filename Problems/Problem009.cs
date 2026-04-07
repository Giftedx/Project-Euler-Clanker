namespace Project_Euler.Problems;

public class Problem009 : Problem {
    public override object Solve() {
        return PythagoreanTripletProduct(1000);
    }

    private int PythagoreanTripletProduct(int s) {
        // From a²+b²=c² and a+b+c=s: b = s(s-2a) / (2(s-a))
        for (int a = 1; a < s / 3; a++) {
            int num = s * (s - 2 * a);
            int den = 2 * (s - a);
            if (num % den != 0) continue;
            int b = num / den;
            int c = s - a - b;
            return a * b * c;
        }
        return 0;
    }
}