namespace Project_Euler.Problems;

public class Problem010 : Problem {
    private const int Limit = 2000000;
    private readonly bool[] _isPrime = Library.GetSieve(Limit);

    public override object Solve() {
        return SumPrimesBelow();
    }

    private long SumPrimesBelow() {
        long sum = 2;
        for (int i = 3; i < Limit; i += 2)
            if (_isPrime[i])
                sum += i;
        return sum;
    }
}