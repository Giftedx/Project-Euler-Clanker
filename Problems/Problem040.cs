namespace Project_Euler.Problems;

public class Problem040 : Problem {
    public override object Solve() {
        return DigitProduct();
    }

    private long DigitProduct() {
        int p = 1;
        for (int x = 1; x <= 1000000; x *= 10)
            p *= FindInFraction(x);
        return p;
    }

    private int FindInFraction(int pos) {
        int length = 1, count = 9, start = 1;

        while (pos > length * count) {
            pos -= length * count;
            length++;
            count *= 10;
            start *= 10;
        }

        int number = start + (pos - 1) / length;

        int fromLeft = (pos - 1) % length;
        int fromRight = length - fromLeft - 1;
        for (int i = 0; i < fromRight; i++)
            number /= 10;


        return number % 10;
    }
}