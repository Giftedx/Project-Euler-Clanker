namespace Project_Euler.Problems;

public class Problem063 : Problem {
    public override object Solve() {
        return PowerfulDigitCount();
    }

    // a^b has exactly b digits iff 10^(b-1) <= a^b < 10^b.
    // Right side: a^b < 10^b <=> (a/10)^b < 1 — only possible when a <= 9.
    // Left side: (b-1)*log10(10) <= b*log10(a) <=> b*(1 - log10(a)) <= 1 <=> b <= 1 / (1 - log10(a)).
    // a=1 gives only b=1; a=9 gives up to b=21. Direct count by iteration.
    private int PowerfulDigitCount() {
        int total = 0;
        for (int a = 1; a <= 9; a++) {
            for (int b = 1; ; b++) {
                int digits = (int)Math.Floor(b * Math.Log10(a)) + 1;
                if (digits < b) break;
                if (digits == b) total++;
            }
        }
        return total;
    }
}
