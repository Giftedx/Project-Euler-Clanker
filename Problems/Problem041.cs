namespace Project_Euler.Problems;

public class Problem041 : Problem {
    private bool _found;
    private int _result;

    public override object Solve() {
        return MaxPandigitalPrime();
    }

    private int MaxPandigitalPrime() {
        for (int digits = 9; digits >= 2; digits--) {
            if (digits * (digits + 1) / 2 % 3 == 0) continue;

            int[] perm = new int[digits];
            for (int i = 0; i < perm.Length; i++) perm[i] = perm.Length - i;

            GeneratePrimePermutations(perm, 0);
            if (_found) return _result;
        }

        return -1;
    }

    private void GeneratePrimePermutations(int[] perm, int l) {
        if (_found) return;
        int r = perm.Length - 1;

        if (l == r) {
            int num = 0;
            for (int i = 0; i <= r; i++)
                num = num * 10 + perm[i];
            if (!Library.IsPrime(num)) return;
            _result = num;
            _found = true;
            return;
        }

        for (int i = l; i <= r; i++) {
            (perm[l], perm[i]) = (perm[i], perm[l]);
            GeneratePrimePermutations(perm, l + 1);
            (perm[l], perm[i]) = (perm[i], perm[l]);
            if (_found) return;
        }
    }
}