namespace Project_Euler.Problems;

public class Problem024 : Problem {
    public override object Solve() {
        return NthLexicalPermutation(1000000);
    }

    private string NthLexicalPermutation(ulong target) {
        char[] digits = "0123456789".ToCharArray();
        char[] number = "          ".ToCharArray();
        target--;
        int n = 10;
        int nDigits = 0;

        while (n-- > 0) {
            ulong fn = (ulong)Library.Factorial(n);
            int i = (int)(target / fn);
            target -= (ulong)i * fn;
            number[nDigits++] = digits[i];

            for (int r = i; r < n; r++)
                digits[r] = digits[r + 1];
        }

        return new string(number);
    }
}