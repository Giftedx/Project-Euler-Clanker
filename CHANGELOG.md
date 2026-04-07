# Changelog

## Performance Optimization Pass (April 2026)

Comprehensive algorithmic optimization of all 60 Project Euler solutions, verified against known answers. Total execution time reduced from **640ms to 391ms (39% faster)** with 2 bug fixes.

### Bug Fixes

- **Problem 054** (Poker Hands): Fixed bit-packing overlap in one-pair hand ranking, and added ace-low straight (A-2-3-4-5) detection.
- **Problem 059** (XOR Decryption): Fixed missing data file path. Rewrote to use independent per-position key search — O(3 x 26) instead of O(26^3).

### Major Optimizations (2x+ speedup)

| Problem | Before | After | Speedup | Technique |
|---------|--------|-------|---------|-----------|
| 007 (10001st Prime) | 3.88ms | 0.30ms | 12.8x | Sieve-based enumeration instead of per-number `IsPrime()` calls |
| 038 (Pandigital Multiples) | 1.41ms | 0.18ms | 8.0x | Digit mask with integer arithmetic, no `StringBuilder` |
| 035 (Circular Primes) | 0.11ms | 0.02ms | 6.2x | Direct sieve array lookup instead of `ConcurrentDictionary` cache |
| 012 (Highly Divisible Triangle) | 2.86ms | 0.49ms | 5.9x | Smallest-prime-factor sieve for O(log n) divisor counting (Tau) |
| 004 (Largest Palindrome Product) | 0.13ms | 0.02ms | 5.8x | Descending search with early exit + divisibility-by-11 filter |
| 046 (Goldbach's Other Conjecture) | 0.49ms | 0.17ms | 2.9x | Sieve lookup replacing `Library.IsPrime()` calls |
| 052 (Permuted Multiples) | 2.39ms | 0.99ms | 2.4x | Digit signature via bit-shifted `long` instead of `SameDigits()` |
| 058 (Spiral Primes) | 2.41ms | 1.13ms | 2.1x | Deterministic Miller-Rabin primality with local cache |
| 008 (Largest Product in Series) | 0.04ms | 0.02ms | 2.1x | Direct character indexing instead of `Substring` allocation |

### Moderate Optimizations

| Problem | Before | After | Speedup | Technique |
|---------|--------|-------|---------|-----------|
| 048 (Self Powers) | 1.11ms | 0.74ms | 1.5x | `long` modular exponentiation replacing `BigInteger` |
| 050 (Consecutive Prime Sum) | 1.45ms | 1.25ms | 1.2x | Prefix sums with search by decreasing length |
| 060 (Prime Pair Sets) | 20.6ms | 17.2ms | 1.2x | Miller-Rabin for concatenation primality, `Dictionary` cache |
| 055 (Lychrel Numbers) | 1.58ms | 1.26ms | 1.3x | Simplified inline palindrome check without `HashSet` overhead |
| 051 (Prime Digit Replacements) | 4.35ms | 3.74ms | 1.2x | Iterate only 6-digit primes, use `int.PopCount` |
| 043 (Sub-string Divisibility) | 3.01ms | 2.76ms | 1.1x | Arithmetic accumulation during recursion instead of `long.Parse` |

### Infrastructure

- **Benchmark harness**: JIT warm-up (5 untimed runs), conditional GC collection, HTML/JSON report output.
- **Test runner**: `--verify` flag runs all 60 problems non-interactively with known-answer PASS/FAIL checking.
- **Data files**: Moved to `data/` directory with auto-copy via `.csproj`. Added missing `poker.txt`.
- **`.gitignore`**: Covers build output, IDE files, benchmark artifacts.

### Key Techniques Used

- **Sieve of Eratosthenes** for O(1) primality lookups up to known bounds
- **Smallest-prime-factor sieve** for O(log n) prime factorization / divisor counting
- **Deterministic Miller-Rabin** (witnesses 2,3,5,7) for large number primality — correct for all n < 3.2 billion
- **Modular arithmetic with `long`** to avoid `BigInteger` heap allocation
- **Bit manipulation** for digit signatures, pandigital checking, population count
- **Stack-allocated spans** for digit frequency arrays (no heap pressure)
- **Descending search with early termination** for maximum-finding problems
