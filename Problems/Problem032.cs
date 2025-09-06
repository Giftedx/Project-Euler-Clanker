namespace Project_Euler.Problems;

public class Problem032 : Problem {
    public override object Solve() {
        return SumPandigitalProducts();
    }

    private int SumPandigitalProducts() {
        HashSet<int> products = [];

        for (int a = 1; a < 10; a++)
        for (int b = 1234; b < 9877; b++) {
            int c = a * b;
            if (c > 9999) continue;
            if (IsPandigital(a, b, c)) products.Add(c);
        }

        for (int a = 12; a < 100; a++)
        for (int b = 123; b < 988; b++) {
            int c = a * b;
            if (c > 9999) continue;
            if (IsPandigital(a, b, c)) products.Add(c);
        }

        return products.Sum();
    }

    private bool IsPandigital(int a, int b, int c) {
        Span<byte> digits = stackalloc byte[10];
        int totalDigits = 0;

        if (!CheckDigits(a, digits, ref totalDigits)) return false;
        if (!CheckDigits(b, digits, ref totalDigits)) return false;
        if (!CheckDigits(c, digits, ref totalDigits)) return false;

        return totalDigits == 9;
    }

    private bool CheckDigits(int number, Span<byte> digits, ref int total) {
        while (number > 0) {
            int d = number % 10;
            if (d == 0 || digits[d]++ > 0) return false;
            number /= 10;
            total++;
        }

        return true;
    }
}