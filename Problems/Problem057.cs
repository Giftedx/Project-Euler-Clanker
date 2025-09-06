using System.Numerics;

namespace Project_Euler.Problems;

public class Problem057 : Problem{
    public override object Solve() {
        return SquareConvergents();
    }

    private int SquareConvergents() {
        BigInteger n = 0;
        BigInteger d = 1;
        int count = 0;
        for(int i = 0; i < 1000; i++) {
            (n, d) = (d, d * 2 + n);
            if(Math.Floor(BigInteger.Log10(n + d) + 1) > 
               Math.Floor(BigInteger.Log10(d) + 1) ) count++;
        }
        return count;
    }
}