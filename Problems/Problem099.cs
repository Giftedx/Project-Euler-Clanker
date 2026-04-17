namespace Project_Euler.Problems;

public class Problem099 : Problem {
    public override object Solve() {
        return LargestExponentialLineNumber();
    }

    // Compare b^e pairs via exp * log(base). No big-number math needed.
    private int LargestExponentialLineNumber() {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "p099_base_exp.txt");
        double bestVal = double.MinValue;
        int bestLine = 0;
        int lineNum = 0;
        foreach (string line in File.ReadAllLines(path)) {
            lineNum++;
            string[] parts = line.Split(',');
            int b = int.Parse(parts[0]);
            int e = int.Parse(parts[1]);
            double v = e * Math.Log(b);
            if (v <= bestVal) continue;
            bestVal = v;
            bestLine = lineNum;
        }
        return bestLine;
    }
}
