namespace Project_Euler.Problems;

public class Problem036 : Problem {
    public override object Solve() {
        return DoubleBasePalindromeSum(1000000);
    }

    private long DoubleBasePalindromeSum(int n) {
        long sum = 0;

        for (int len = 1; len <= 6; len++) {
            bool oddLength = len % 2 == 1;
            int halfLen = (len + 1) >> 1;
            int start = Library.Pow10(halfLen - 1);
            int end = Library.Pow10(halfLen);

            for (int i = start; i < end; i++) {
                int p = MakePalindrome(i, oddLength);
                if (p < n && IsBinaryPalindrome(p)) sum += p;
            }
        }

        return sum;
    }

    private int MakePalindrome(int half, bool oddLength) {
        int result = half;
        if (oddLength) half /= 10;

        while (half > 0) {
            result = result * 10 + half % 10;
            half /= 10;
        }

        return result;
    }

    private bool IsBinaryPalindrome(int n) {
        int reversed = 0, temp = n;
        while (temp > 0) {
            reversed = (reversed << 1) | (temp & 1);
            temp >>= 1;
        }

        return n == reversed;
    }
}