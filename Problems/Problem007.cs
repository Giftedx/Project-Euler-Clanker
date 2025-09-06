namespace Project_Euler.Problems;

public class Problem007 : Problem {
    public override object Solve() {
        return NthPrime(10001);
    }

    private int NthPrime(int n) {
        int i = 2;
        while (n > 0) {
            if (Library.IsPrime(i)) n--;
            i++;
        }

        return i - 1;
    }
}