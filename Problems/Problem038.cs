namespace Project_Euler.Problems;

public class Problem038 : Problem {
    public override object Solve() {
        return PandigitalMultiples();
    }

    private static int PandigitalMultiples() {
        int max = 0;

        // n=2: multiply by 1,2 → need 9 digits total. i can be up to 9999 (4+5=9 digits)
        // But for max result, i should be 9xxx (starts with 9)
        // n=3: i up to 333 (3+3+3=9), n=4: i up to 33 (2+2+2+3=9), etc.
        for (int n = 2; n <= 9; n++) {
            int limit = Library.Pow10(9 / n);
            for (int i = 1; i < limit; i++) {
                int result = 0;
                int digitCount = 0;
                int digitMask = 0;
                bool valid = true;

                for (int j = 1; j <= n; j++) {
                    int product = i * j;
                    int prodDigits = Library.NumDigits(product);
                    digitCount += prodDigits;
                    if (digitCount > 9) { valid = false; break; }

                    // Check each digit: no zeros, no repeats
                    int temp = product;
                    while (temp > 0) {
                        int d = temp % 10;
                        if (d == 0) { valid = false; break; }
                        int bit = 1 << d;
                        if ((digitMask & bit) != 0) { valid = false; break; }
                        digitMask |= bit;
                        temp /= 10;
                    }
                    if (!valid) break;

                    // Accumulate the concatenated number
                    result = result * Library.Pow10(prodDigits) + product;
                }

                if (valid && digitCount == 9 && result > max)
                    max = result;
            }
        }

        return max;
    }
}
