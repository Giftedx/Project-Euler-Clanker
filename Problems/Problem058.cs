namespace Project_Euler.Problems;

public class Problem058 : Problem{
    public override object Solve() {
        return SpiralPrimes();
    }

    private int SpiralPrimes() {
        int primeCount = 0;
        int totalDiagonals = 1; // Start with center '1'
        int sideLength = 1;

        while (sideLength == 1 || (double)primeCount / totalDiagonals >= 0.10) {
            sideLength += 2;
            int square = sideLength * sideLength;

            for (int i = 1; i <= 3; i++) {
                int cornerValue = square - i * (sideLength - 1);
                if (Library.IsPrime(cornerValue)) primeCount++;
            }

            totalDiagonals += 4;
        }

        return sideLength;
    }
    /*{
                int primeCount = 0;
                int totalDiagonals = 1;
                int sideLength = 1;

                while (sideLength == 1 || (double)primeCount / totalDiagonals >= 0.10) {
                    sideLength += 2;
                    int square = sideLength * sideLength;

                    for (int i = 1; i <= 3; i++) {
                        int cornerValue = square - i * (sideLength - 1);
                        //Console.WriteLine(cornerValue);
                        if (_isPrime[cornerValue]) primeCount++;
                    }

                    totalDiagonals += 4;
                }

                return sideLength;
            }*/
}