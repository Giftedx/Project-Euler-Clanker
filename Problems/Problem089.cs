namespace Project_Euler.Problems;

public class Problem089 : Problem {
    public override object Solve() {
        return TotalRomanCharSavings();
    }

    // Decode each numeral to int, re-encode minimally, sum difference in length.
    private int TotalRomanCharSavings() {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "p089_roman.txt");
        int savings = 0;
        foreach (string line in File.ReadAllLines(path)) {
            string s = line.Trim();
            if (s.Length == 0) continue;
            int value = RomanToInt(s);
            string minimal = IntToRoman(value);
            savings += s.Length - minimal.Length;
        }
        return savings;
    }

    private static int RomanToInt(string s) {
        int total = 0, prev = 0;
        for (int i = s.Length - 1; i >= 0; i--) {
            int v = Value(s[i]);
            total += v < prev ? -v : v;
            prev = v;
        }
        return total;
    }

    private static int Value(char c) => c switch {
        'I' => 1, 'V' => 5, 'X' => 10, 'L' => 50, 'C' => 100, 'D' => 500, 'M' => 1000, _ => 0
    };

    private static readonly int[] Vals = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
    private static readonly string[] Syms = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };

    private static string IntToRoman(int n) {
        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < Vals.Length; i++) {
            while (n >= Vals[i]) { sb.Append(Syms[i]); n -= Vals[i]; }
        }
        return sb.ToString();
    }
}
