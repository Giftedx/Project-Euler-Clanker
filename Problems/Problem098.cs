namespace Project_Euler.Problems;

public class Problem098 : Problem {
    public override object Solve() {
        return LargestAnagramicSquare();
    }

    // Group words by sorted-letter signature; each pair in a group is an anagram pair.
    // For each pair, for each square with matching length, check if the digit assignment derived
    // from word1->square extended to word2 yields a square too. Track largest pair-square.
    private long LargestAnagramicSquare() {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "p098_words.txt");
        string raw = File.ReadAllText(path);
        var words = raw.Replace("\"", "").Split(',');

        var groups = new Dictionary<string, List<string>>();
        foreach (string w in words) {
            string key = string.Concat(w.OrderBy(c => c));
            if (!groups.TryGetValue(key, out var list)) {
                list = new List<string>();
                groups[key] = list;
            }
            list.Add(w);
        }

        // Pre-generate squares by length
        var squaresByLen = new Dictionary<int, List<long>>();
        for (long n = 1; n * n < 1_000_000_000_000L; n++) {
            long sq = n * n;
            int len = sq.ToString().Length;
            if (!squaresByLen.TryGetValue(len, out var list)) {
                list = new List<long>();
                squaresByLen[len] = list;
            }
            list.Add(sq);
        }

        long best = 0;
        foreach (var kv in groups) {
            if (kv.Value.Count < 2) continue;
            int len = kv.Key.Length;
            if (!squaresByLen.TryGetValue(len, out var squares)) continue;

            for (int i = 0; i < kv.Value.Count; i++) {
                for (int j = i + 1; j < kv.Value.Count; j++) {
                    string w1 = kv.Value[i], w2 = kv.Value[j];
                    foreach (long s1 in squares) {
                        string s1s = s1.ToString();
                        var map = BuildMap(w1, s1s);
                        if (map == null) continue;
                        string s2 = ApplyMap(w2, map);
                        if (s2[0] == '0') continue;
                        long s2v = long.Parse(s2);
                        long r = (long)Math.Sqrt(s2v);
                        if (r * r != s2v && (r + 1) * (r + 1) != s2v) continue;
                        long candidate = Math.Max(s1, s2v);
                        if (candidate > best) best = candidate;
                    }
                }
            }
        }
        return best;
    }

    private static Dictionary<char, char>? BuildMap(string word, string num) {
        var map = new Dictionary<char, char>();
        var used = new HashSet<char>();
        for (int i = 0; i < word.Length; i++) {
            char w = word[i], d = num[i];
            if (map.TryGetValue(w, out char existing)) {
                if (existing != d) return null;
            } else {
                if (!used.Add(d)) return null;
                map[w] = d;
            }
        }
        return map;
    }

    private static string ApplyMap(string word, Dictionary<char, char> map) {
        return string.Concat(word.Select(c => map[c]));
    }
}
