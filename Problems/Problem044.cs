// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Project_Euler.Problems;

public class Problem044 : Problem {
    public override object Solve() {
        return FindMinimumPentagon();
    }

    private int FindMinimumPentagon() {
        int pentagonGap = 1;
        int currentPentagon = 1;

        while (true) {
            pentagonGap += 3;
            currentPentagon += pentagonGap;

            int triangleFactor = 2 + pentagonGap % 9;
            int triangleGap = 3 * triangleFactor + 12;
            int difference = currentPentagon - triangleFactor *
                (triangleFactor - 1) / 6;

            while (triangleFactor <= difference) {
                if (difference % triangleFactor == 0) {
                    int j = difference / triangleFactor;
                    int delta = 1 + 24 * (j * (3 * j - 1) + currentPentagon);
                    int sqrtDelta = (int)Math.Sqrt(delta);

                    if (sqrtDelta * sqrtDelta == delta && sqrtDelta % 6 == 5)
                        return currentPentagon;
                }

                if (difference < triangleGap) break;

                triangleFactor += 9;
                difference -= triangleGap;
                triangleGap += 27;
            }
        }
    }
}