namespace Project_Euler.Problems;

public class Problem004 : Problem {
    public override object Solve() {
        return LargestPalindromeProduct();
    }

    private int LargestPalindromeProduct() {
        int best = 0;
        for (int i = 999; i >= 100; i--) {
            if (i * 999 <= best) break; // No possible product can beat best
            // 6-digit palindrome is divisible by 11, so one factor must be a multiple of 11
            int jStart = (i % 11 == 0) ? 999 : 999 - (999 % 11);
            int jStep = (i % 11 == 0) ? 1 : 11;
            for (int j = jStart; j >= i; j -= jStep) {
                int product = i * j;
                if (product <= best) break;
                if (Library.IsPalindrome(product))
                    best = product;
            }
        }

        return best;
    }
}
