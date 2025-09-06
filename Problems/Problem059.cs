namespace Project_Euler.Problems;

public class Problem059 : Problem{
    public Problem059() {
        string input = File.ReadAllText("0059_cipher.txt");
        _ciphertext = input.Split(',').Select(int.Parse).ToArray();
    }
    
    public override object Solve() {
        return SumAscii();
    }

    private readonly int[] _ciphertext;

    private int SumAscii() {
        int len = _ciphertext.Length;
        int bestSum = 0;
        int maxScore = int.MinValue;
        object lockObj = new();

        Parallel.For(97, 123, a => {
            for (int b = 97; b <= 122; b++) {
                for (int c = 97; c <= 122; c++) {
                    int score = 0;
                    int sum = 0;

                    for (int i = 0; i < len; i++) {
                        int key = (i % 3 == 0) ? a : (i % 3 == 1) ? b : c;
                        int decodedChar = _ciphertext[i] ^ key;
                        score += ScoreChar(decodedChar);
                        sum += decodedChar;
                    }

                    lock (lockObj) {
                        if (score <= maxScore) continue;
                        maxScore = score;
                        bestSum = sum;
                    }
                }
            }
        });

        return bestSum;

        int ScoreChar(int c) => c switch {
            >= 'a' and <= 'z' => 2,
            >= 'A' and <= 'Z' => 1,
            ' ' => 3,
            '.' or ',' or '\'' or ';' => 1,
            _ => -1
        };
    }
}