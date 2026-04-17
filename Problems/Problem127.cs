namespace Project_Euler.Problems;

public class Problem127 : Problem {
    public override object Solve() {
        return SumAbcHitsBelow(120_000);
    }

    // a+b=c with a<b, gcd(a,b)=1, rad(a)*rad(b)*rad(c) < c.
    // Because a,b coprime => c coprime with both => rad(abc)=rad(a)*rad(b)*rad(c).
    // Iterate indices rad-sorted. For each i, iterate j > i; break when rad[i]*rad[j] >= limit
    // (since then rad(a)*rad(b)*rad(c) >= rad[i]*rad[j] >= limit > c).
    private long SumAbcHitsBelow(int limit) {
        int[] rad = new int[limit];
        for (int i = 0; i < limit; i++) rad[i] = 1;
        for (int p = 2; p < limit; p++) {
            if (rad[p] != 1) continue;
            for (int j = p; j < limit; j += p) rad[j] *= p;
        }

        int[] sortedByRad = Enumerable.Range(1, limit - 1)
            .OrderBy(x => rad[x])
            .ToArray();

        long total = 0;
        for (int i = 0; i < sortedByRad.Length; i++) {
            int x = sortedByRad[i];
            int rx = rad[x];
            for (int j = i + 1; j < sortedByRad.Length; j++) {
                int y = sortedByRad[j];
                long radXY = (long)rx * rad[y];
                if (radXY >= limit) break;
                int a = Math.Min(x, y), b = Math.Max(x, y);
                int c = a + b;
                if (c >= limit) continue;
                if (radXY * (long)rad[c] >= c) continue;
                if (Library.Gcd(a, b) != 1) continue;
                total += c;
            }
        }
        return total;
    }
}
