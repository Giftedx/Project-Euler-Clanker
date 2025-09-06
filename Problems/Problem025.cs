namespace Project_Euler.Problems;

public class Problem025 : Problem {
    public override object Solve() {
        return FibonacciNDigits(1000);
    }

    private int FibonacciNDigits(int n) {
        double phi = (1 + Math.Sqrt(5)) / 2;
        double log10Phi = Math.Log10(phi); // log10(φ)
        double log10Sqrt5 = Math.Log10(Math.Sqrt(5)); // log10(√5)

        int index = 1;
        while (true) {
            double logFib = index * log10Phi - log10Sqrt5;
            int digitCount = (int)Math.Floor(logFib) + 1;
            if (digitCount >= n) break;
            index++;
        }

        return index;
    }
}