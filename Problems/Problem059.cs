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
        // Precompute score lookup table
        int[] scoreTable = new int[256];
        Array.Fill(scoreTable, -1);
        for (int i = 'a'; i <= 'z'; i++) scoreTable[i] = 2;
        for (int i = 'A'; i <= 'Z'; i++) scoreTable[i] = 1;
        scoreTable[' '] = 3;
        scoreTable['.'] = 1; scoreTable[','] = 1; scoreTable['\''] = 1; scoreTable[';'] = 1;
        scoreTable['\n'] = 0; scoreTable['\r'] = 0; scoreTable['\t'] = 0;

        int len = _ciphertext.Length;
        // Split ciphertext by position mod 3 for independent key search
        // This reduces from 26^3 to 3 * 26 independent checks
        int[][] groups = new int[3][];
        for (int g = 0; g < 3; g++) {
            int count = (len - g + 2) / 3;
            groups[g] = new int[count];
            for (int i = 0; i < count; i++) groups[g][i] = _ciphertext[g + i * 3];
        }

        int[] bestKeys = new int[3];
        for (int g = 0; g < 3; g++) {
            int bestScore = int.MinValue;
            for (int key = 97; key <= 122; key++) {
                int score = 0;
                foreach (int ch in groups[g]) {
                    int decoded = ch ^ key;
                    score += decoded < 256 ? scoreTable[decoded] : -1;
                }
                if (score > bestScore) {
                    bestScore = score;
                    bestKeys[g] = key;
                }
            }
        }

        // Compute sum with best keys
        int sum = 0;
        for (int i = 0; i < len; i++) {
            sum += _ciphertext[i] ^ bestKeys[i % 3];
        }
        return sum;
    }
}
