using static Project_Euler.Library;

namespace Project_Euler.Problems;

public class Problem052 : Problem {
    public override object Solve() {
        return SmallestPermutedMult();
    }

    private readonly int[] _mults = new int[5];

    private int SmallestPermutedMult() {
        int n = 100;
        while (true) {
            if (NumDigits(n) != NumDigits(n * 6)) {
                n = (int)Math.Pow(10, NumDigits(n));
                continue;
            }
            for (int i = 2; i < 7; i++) {
                _mults[i - 2] = n * i;
            }
            if(MultTest(n))return n;
            n++;
        }
    }

    private bool MultTest(int n) {
        foreach (int i in _mults) if(!SameDigits(n, i))return false;
        return true;
    }
}