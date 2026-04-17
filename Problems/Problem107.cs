namespace Project_Euler.Problems;

public class Problem107 : Problem {
    public override object Solve() {
        return MaxSavingsFromMST();
    }

    // Parse symmetric adjacency matrix (dashes for no edge), run Kruskal's MST, subtract from total.
    private int MaxSavingsFromMST() {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "p107_network.txt");
        string[] lines = File.ReadAllLines(path);
        int n = lines.Length;

        var edges = new List<(int w, int u, int v)>();
        int totalWeight = 0;
        for (int i = 0; i < n; i++) {
            string[] parts = lines[i].Split(',');
            for (int j = i + 1; j < n; j++) {
                if (parts[j] == "-") continue;
                int w = int.Parse(parts[j]);
                edges.Add((w, i, j));
                totalWeight += w;
            }
        }

        edges.Sort((a, b) => a.w.CompareTo(b.w));
        int[] parent = new int[n];
        for (int i = 0; i < n; i++) parent[i] = i;

        int mstWeight = 0;
        foreach (var (w, u, v) in edges) {
            int ru = Find(parent, u), rv = Find(parent, v);
            if (ru == rv) continue;
            parent[ru] = rv;
            mstWeight += w;
        }
        return totalWeight - mstWeight;
    }

    private static int Find(int[] parent, int x) {
        while (parent[x] != x) {
            parent[x] = parent[parent[x]];
            x = parent[x];
        }
        return x;
    }
}
