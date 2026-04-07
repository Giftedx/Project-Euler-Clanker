namespace Project_Euler.Problems;

public class Problem055 : Problem {
    private const int Limit = 10000;

    public override object Solve() {
        return LychrelCountBelow(Limit);
    }

    private static int LychrelCountBelow(int n) {
        int count = 0;
        for (int i = 10; i < n; i++) {
            if (IsLychrel(i)) count++;
        }
        return count;
    }

    private static bool IsLychrel(int n) {
        long num = n;
        for (int i = 0; i < 50; i++) {
            num += ReverseDigits(num);
            if (IsPalindromeNum(num)) return false;
        }
        return true;
    }

    private static long ReverseDigits(long n) {
        long rev = 0;
        while (n > 0) {
            rev = rev * 10 + n % 10;
            n /= 10;
        }
        return rev;
    }

    private static bool IsPalindromeNum(long n) {
        return n == ReverseDigits(n);
    }
}
