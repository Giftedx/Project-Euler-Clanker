namespace Project_Euler.Problems;

public class Problem017 : Problem {
    public override object Solve() {
        return NumberLetterCount();
    }

    private int NumberLetterCount() {
        int digits = CountLetters("onetwothreefourfivesixseveneightnine");
        int teens = CountLetters("teneleventwelvethirteenfourteenfifteen" +
                                 "sixteenseventeeneighteennineteen");
        int tens = CountLetters("twentythirtyfortyfiftysixtyseventyeightyninety");
        int hundred = CountLetters("hundred");
        int and = CountLetters("and");
        int oneThousand = CountLetters("onethousand");

        int oneToNinetyNine = digits + teens + 8 * digits + 10 * tens;
        int hundredsPart = 100 * digits;
        int andPart = 99 * 9 * and;
        int subHundredPart = 9 * oneToNinetyNine;
        int hundredText = 9 * 100 * hundred;

        return oneToNinetyNine + hundredsPart + andPart +
               subHundredPart + hundredText + oneThousand;
    }

    private static int CountLetters(string s) {
        return s.Count(char.IsLetter);
    }
}