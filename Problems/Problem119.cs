using System.Numerics;

namespace Project_Euler.Problems;

public class Problem119 : Problem {
    public override object Solve() {
        return ThirtiethDigitPower();
    }

    // Generate candidates b^e for small b, e; filter those where digit-sum of b^e equals b; collect distinct, sort, return 30th.
    private long ThirtiethDigitPower() {
        var candidates = new SortedSet<BigInteger>();
        for (int b = 2; b <= 100; b++) {
            BigInteger pow = b;
            for (int e = 2; e <= 100; e++) {
                pow *= b;
                if (pow < 10) continue;
                if (Library.SumDigits(pow) == b) candidates.Add(pow);
                if (candidates.Count > 200) break;
            }
        }
        int i = 0;
        foreach (var v in candidates) {
            i++;
            if (i == 30) return (long)v;
        }
        return -1;
    }
}
