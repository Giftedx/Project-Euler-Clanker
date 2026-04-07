namespace Project_Euler.Problems;

public class Problem032 : Problem {

    public override object Solve() {
        return SumPandigitalProducts();
    }

    private static int SumPandigitalProducts() {
        HashSet<int> products = [];

        for (int a = 1; a < 10; a++)
        for (int b = 1234; b < 9877; b++) {
            int c = a * b;
            if (c > 9999) break;
            if (IsPandigital(a, b, c)) products.Add(c);
        }

        for (int a = 12; a < 100; a++)
        for (int b = 123; b < 988; b++) {
            int c = a * b;
            if (c > 9999) break;
            if (IsPandigital(a, b, c)) products.Add(c);
        }

        int sum = 0;
        foreach (int p in products) sum += p;
        return sum;
    }

    private static bool IsPandigital(int a, int b, int c) {
        int mask = 0;
        int count = 0;

        if (!AddDigits(a, ref mask, ref count)) return false;
        if (!AddDigits(b, ref mask, ref count)) return false;
        if (!AddDigits(c, ref mask, ref count)) return false;

        return count == 9 && mask == 0x3FE; // bits 1-9 set
    }

    private static bool AddDigits(int n, ref int mask, ref int count) {
        while (n > 0) {
            int d = n % 10;
            if (d == 0) return false;
            int bit = 1 << d;
            if ((mask & bit) != 0) return false;
            mask |= bit;
            count++;
            n /= 10;
        }
        return true;
    }
}
