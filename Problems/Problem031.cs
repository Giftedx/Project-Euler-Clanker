namespace Project_Euler.Problems;

public class Problem031 : Problem {
    private readonly int[] _coins = [1, 2, 5, 10, 20, 50, 100, 200];

    public override object Solve() {
        return CoinCombosForN(200);
    }

    private int CoinCombosForN(int n) {
        int[] combos = new int[n + 1];
        combos[0] = 1;

        foreach (int coin in _coins)
            for (int i = coin; i <= n; i++)
                combos[i] += combos[i - coin];

        return combos[n];
    }
}