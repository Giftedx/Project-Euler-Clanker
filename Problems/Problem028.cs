namespace Project_Euler.Problems;

public class Problem028 : Problem {
    public override object Solve() {
        return SpiralSum(1001);
    }

    private int SpiralSum(int size) {
        int n = 1;
        int step = 2;
        int total = 0;
        int ringStep = 0;
        while (n <= size * size) {
            total += n;
            n += step;
            ringStep++;
            if (ringStep != 4) continue;
            step += 2;
            ringStep = 0;
        }

        return total;
    }
}