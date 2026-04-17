namespace Project_Euler.Problems;

public class Problem122 : Problem {
    public override object Solve() {
        return SumMinChainLengths(200);
    }

    // Single DFS with depth limit ~11; at each visited node, record min depth to reach that value.
    // Branching in the addition-chain tree stays manageable thanks to next > prev pruning.
    private int SumMinChainLengths(int N) {
        int[] best = new int[N + 1];
        for (int i = 2; i <= N; i++) best[i] = int.MaxValue;
        best[1] = 0;

        const int maxDepth = 11;
        int[] chain = new int[maxDepth + 1];
        chain[0] = 1;
        Dfs(0, N, chain, best, maxDepth);

        int sum = 0;
        for (int k = 1; k <= N; k++) sum += best[k];
        return sum;
    }

    private static void Dfs(int depth, int N, int[] chain, int[] best, int maxDepth) {
        if (depth >= maxDepth) return;
        int prev = chain[depth];
        for (int i = depth; i >= 0; i--) {
            for (int j = i; j >= 0; j--) {
                int next = chain[i] + chain[j];
                if (next <= prev) continue;
                if (next > N) continue;
                if (depth + 1 > best[next]) continue;
                chain[depth + 1] = next;
                if (depth + 1 < best[next]) best[next] = depth + 1;
                Dfs(depth + 1, N, chain, best, maxDepth);
            }
        }
    }
}
