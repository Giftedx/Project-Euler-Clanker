namespace Project_Euler.Problems;

public class Problem052 : Problem {
    public override object Solve() {
        return SmallestPermutedMult();
    }

    private static int SmallestPermutedMult() {
        int n = 100;
        while (true) {
            // Skip to next power of 10 range if 6x would have more digits
            if (Library.NumDigits(n) != Library.NumDigits(n * 6)) {
                n = Library.Pow10(Library.NumDigits(n));
                continue;
            }

            long sig = DigitSignature(n);
            bool match = true;
            for (int i = 2; i <= 6; i++) {
                if (DigitSignature(n * i) != sig) { match = false; break; }
            }
            if (match) return n;
            n++;
        }
    }

    // Encode digit frequencies into a long: 4 bits per digit (0-9)
    private static long DigitSignature(int num) {
        long sig = 0;
        while (num > 0) {
            sig += 1L << ((num % 10) * 4);
            num /= 10;
        }
        return sig;
    }
}
