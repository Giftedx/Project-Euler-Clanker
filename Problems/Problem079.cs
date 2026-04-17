namespace Project_Euler.Problems;

public class Problem079 : Problem {
    public override object Solve() {
        return ShortestPasscode();
    }

    // Build a precedence DAG from 3-digit login attempts (a before b, b before c) and topologically sort.
    // The problem's input is known to have no duplicate digits, so the topological order IS the passcode.
    private long ShortestPasscode() {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "p079_keylog.txt");
        var edges = new HashSet<(int, int)>();
        var seenDigits = new HashSet<int>();

        foreach (string line in File.ReadAllLines(path)) {
            if (line.Length < 3) continue;
            int a = line[0] - '0', b = line[1] - '0', c = line[2] - '0';
            seenDigits.Add(a); seenDigits.Add(b); seenDigits.Add(c);
            edges.Add((a, b));
            edges.Add((b, c));
            edges.Add((a, c));
        }

        var inDeg = new Dictionary<int, int>();
        var adj = new Dictionary<int, List<int>>();
        foreach (int d in seenDigits) { inDeg[d] = 0; adj[d] = new List<int>(); }
        foreach (var (u, v) in edges) { adj[u].Add(v); inDeg[v]++; }

        var queue = new Queue<int>();
        foreach (var kv in inDeg) if (kv.Value == 0) queue.Enqueue(kv.Key);
        long code = 0;
        while (queue.Count > 0) {
            int n = queue.Dequeue();
            code = code * 10 + n;
            foreach (int next in adj[n]) {
                if (--inDeg[next] == 0) queue.Enqueue(next);
            }
        }
        return code;
    }
}
