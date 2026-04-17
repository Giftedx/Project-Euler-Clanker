namespace Project_Euler.Problems;

public class Problem073 : Problem {
    public override object Solve() {
        return CountFractionsInRange(12_000);
    }

    // Stern-Brocot / mediant traversal between 1/3 and 1/2 with denominator bound.
    // Stack-based in-order walk; counts every reduced fraction strictly between the endpoints.
    private int CountFractionsInRange(int maxDen) {
        int count = 0;
        var stack = new Stack<(int a, int b, int c, int d)>();
        stack.Push((1, 3, 1, 2));
        while (stack.Count > 0) {
            var (a, b, c, d) = stack.Pop();
            int m = a + c, n = b + d;
            if (n > maxDen) continue;
            count++;
            stack.Push((a, b, m, n));
            stack.Push((m, n, c, d));
        }
        return count;
    }
}
