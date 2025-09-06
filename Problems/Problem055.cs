namespace Project_Euler.Problems;

public class Problem055 : Problem {
    private const int Limit = 10000;
    public override object Solve() {
        return LychrelCountBelow(Limit);
    }
    
    private readonly HashSet<long> _nonLychrel = [];
    private readonly HashSet<long> _lychrels = [];

    private int LychrelCountBelow(int n) {
        int count = 0;
        for (int i = 10; i < n; i++) {
            if(IsLychrel(i))count++;
        }
        return count;
    }

    private bool IsLychrel(int n) {
        long num = n;
        var seen = new List<long>();
        for (int i = 0; i < 50; i++) {
            num += ReverseDigits(num);
            if (_nonLychrel.Contains(num) || Library.IsPalindrome(num)) {
                CacheList(seen, _nonLychrel);
                return false;
            }

            if (_lychrels.Contains(num)) {
                CacheList(seen, _lychrels);
                return true;
            }
            seen.Add(num);
        }
        CacheList(seen, _lychrels);
        return true;
    }
    
    private void CacheList(List<long> numbers, HashSet<long> cache) {
        foreach (long number in numbers) cache.Add(number);
    }
    
    private long ReverseDigits(long n) {
        long revNum = 0;
        while (n > 0) {
            revNum = revNum * 10 + n % 10;
            n /= 10;
        }
        return revNum;
    }
}