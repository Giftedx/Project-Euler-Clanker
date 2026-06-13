namespace Project_Euler.Problems;

public class Problem138 : Problem {
    public override object Solve() {
        return SumSmallest12L();
    }

    // Isoceles with base b, legs L, h = b/2 ± 1. Leg L = sqrt((b/2)^2 + h^2).
    // L^2 = (b/2)^2 + (b/2 ± 1)^2. Equivalently: b = 2m, L^2 = m^2 + (m ± 1)^2.
    // This leads to Pell equation. Known recurrence: L_{n+1} = 17*L_n - L_{n-1} + 8 (sign-aware).
    // Alternatively, use the closed-form via L values that satisfy 5*L^2 - 4*L + ... = perfect square.
    // Known sequence: 17, 305, 5473, 98209, ..., satisfying L_{n+1} = 18*L_n - L_{n-1} - 16... (OEIS A007805-ish).
    // Pell approach: we want x^2 - 5*y^2 = -4 (y = L, x relates to b). Solutions follow.
    private long SumSmallest12L() {
        // Iterate m; for each m, check if m^2 + (m-1)^2 OR m^2 + (m+1)^2 is a perfect square.
        // Use Pell equation: m^2 + (m±1)^2 = L^2 => 2m^2 ± 2m + 1 = L^2.
        // Rearranging: (2m ± 1)^2 + 1 = 2L^2. Let u = 2m ± 1: u^2 - 2L^2 = -1. Negative Pell.
        // Fundamental (u, L) = (1, 1); next via (u, L) -> (3u + 4L, 2u + 3L). But we need m and L integer with m > 0.
        long total = 0;
        int found = 0;
        long u = 1, L = 1;
        while (found < 12) {
            (u, L) = (3 * u + 4 * L, 2 * u + 3 * L);
            // Extract m: u = 2m - 1 or 2m + 1. Only even (u - 1)/2 or (u + 1)/2 works when u is odd.
            if (u % 2 == 0) continue;
            long m1 = (u - 1) / 2;
            long m2 = (u + 1) / 2;
            // Check which one corresponds to valid L and nonzero base
            foreach (long m in new[] { m1, m2 }) {
                if (m <= 0) continue;
                // Check whether the (base, height) = (2m, m ± 1) give integer L = this L
                if (2 * m * m + 2 * m + 1 == L * L || 2 * m * m - 2 * m + 1 == L * L) {
                    total += L;
                    found++;
                    if (found >= 12) break;
                }
            }
        }
        return total;
    }
}
