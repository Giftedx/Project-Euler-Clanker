namespace Project_Euler.Problems;

public class Problem130 : Problem {
    public override object Solve() {
        return SumFirstCompositeARepunits(25);
    }

    // Composite n with gcd(n, 10) = 1 and A(n) divides n - 1.
    private long SumFirstCompositeARepunits(int count) {
        long total = 0;
        int found = 0;
        for (int n = 3; found < count; n++) {
            if (n % 2 == 0 || n % 5 == 0) continue;
            if (Library.IsPrime(n)) continue;
            int a = Arepunit(n);
            if ((n - 1) % a != 0) continue;
            total += n;
            found++;
        }
        return total;
    }

    private static int Arepunit(int n) {
        long x = 1 % n;
        int k = 1;
        while (x != 0) {
            x = (x * 10 + 1) % n;
            k++;
        }
        return k;
    }
}
