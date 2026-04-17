using System.Numerics;

namespace Project_Euler.Problems;

public class Problem065 : Problem {
    public override object Solve() {
        return ConvergentNumeratorDigitSum(100);
    }

    // e = [2; 1,2,1, 1,4,1, 1,6,1, 1,8,1, ...]
    // a_k (0-indexed): a_0 = 2; for k >= 1, a_k = 2*(k/3 + 1) if k mod 3 == 2, else 1.
    private int ConvergentNumeratorDigitSum(int k) {
        BigInteger pPrev = 1, p = 2; // p_0 = 2
        for (int i = 1; i < k; i++) {
            int a = i % 3 == 2 ? 2 * (i / 3 + 1) : 1;
            (pPrev, p) = (p, a * p + pPrev);
        }
        return Library.SumDigits(p);
    }
}
