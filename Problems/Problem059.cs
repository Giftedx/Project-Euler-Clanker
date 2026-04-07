namespace Project_Euler.Problems;

public class Problem059 : Problem {
    private readonly int[] _ciphertext;

    public Problem059() {
        string input = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "0059_cipher.txt"));
        _ciphertext = input.Split(',').Select(int.Parse).ToArray();
    }

    public override object Solve() {
        return SumAscii();
    }

    private int SumAscii() {
        int len = _ciphertext.Length;
        int bestSum = 0;
        int maxScore = int.MinValue;

        Parallel.For(97, 123, () => (score: int.MinValue, sum: 0),
            (a, _, localBest) => {
                for (int b = 97; b <= 122; b++) {
                    for (int c = 97; c <= 122; c++) {
                        int score = 0;
                        int sum = 0;

                        for (int i = 0; i < len; i++) {
                            int key = (i % 3) switch {
                                0 => a,
                                1 => b,
                                _ => c
                            };
                            int decoded = _ciphertext[i] ^ key;
                            score += ScoreChar(decoded);
                            sum += decoded;
                        }

                        if (score > localBest.score) {
                            localBest = (score, sum);
                        }
                    }
                }
                return localBest;
            },
            localBest => {
                lock (_ciphertext) {
                    if (localBest.score > maxScore) {
                        maxScore = localBest.score;
                        bestSum = localBest.sum;
                    }
                }
            });

        return bestSum;

        static int ScoreChar(int c) => c switch {
            >= 'a' and <= 'z' => 2,
            >= 'A' and <= 'Z' => 1,
            ' ' => 3,
            '.' or ',' or '\'' or ';' => 1,
            _ => -1
        };
    }
}
