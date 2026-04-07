# Performance Optimization Design Spec

**Date:** 2026-04-07
**Approach:** Surgical Strikes (Approach A)
**Scope:** Benchmark infrastructure fixes, Library.cs critical fixes, top 6 problem optimizations

---

## Goals

1. Make benchmark measurements accurate (eliminate JIT noise and GC interference)
2. Fix correctness bug in `Library.SameDigits()`
3. Optimize the shared math library's hottest paths
4. Speed up the 6 problem solutions with the highest optimization potential

**Non-goals:** No new features, no architecture changes, no medium/low priority problems.

---

## Phase 1: Benchmark Infrastructure

Fixes measurement accuracy so all subsequent optimizations can be verified with trustworthy numbers.

### 1a. JIT Warm-up and GC Control

**File:** `BenchmarkConfig.cs`, `Services/ProblemSolver.cs`

Add `WarmupRuns = 5` constant to `BenchmarkConfig`. In `ProblemSolver.Run()`, execute 5 untimed solves before the measurement loop. Call `GC.Collect()` + `GC.WaitForPendingFinalizers()` once after warm-up to start measurements from a clean heap state.

Flow: `warm-up (5 untimed) -> GC.Collect -> measured runs (100 timed)`

**Why:** The JIT compiler compiles methods on first invocation. Without warm-up, the first ~5-10 timed runs include compilation overhead, inflating Best/Average/Median statistics.

### 1b. Fix TotalTime Metric

**File:** `Models/BenchmarkData.cs`, `Services/StatisticsCalculator.cs`

Rename `TotalTime` to `TotalBestTime` in `BenchmarkData` to accurately describe what it represents (sum of each problem's best time). Update `StatisticsCalculator.CalculateBenchmarkData()` and any references in `OutputHandler`.

**Why:** Current name implies wall-clock total but actually sums only best-case times per problem. Misleading for anyone reading the reports.

### 1c. Reduce Progress Thread Polling

**File:** `BenchmarkConfig.cs`

Change `ProgressUpdateIntervalMs` from `10` to `100`.

**Why:** 10ms polling means 100 volatile reads/sec during benchmarks. 100ms is still visually smooth but reduces memory contention by 10x.

---

## Phase 2: Library.cs Critical Fixes

Fixes a bug and optimizes the most-called utility functions.

### 2a. Replace `GetPrimeList()` with Sieve-Based Collection

**File:** `Library.cs` (lines 280-283)

**Current:** Creates a `List<int>` of all integers 1..N, then calls `IsPrime()` on each via `AsParallel().Where(IsPrime)`. For N=1,000,000 this means 1M trial divisions + 4MB list + cache contention.

**Change:** Use the existing `GetSieve()` method and collect primes from the boolean array:
```
sieve = GetSieve(upperLimit)
iterate sieve, add indices where true to primeList
```

**Expected improvement:** ~100x for large limits. Sieve is O(n log log n) vs O(n * sqrt(n)) trial division.

### 2b. Precompute sqrt in `IsPrime()` Loops

**File:** `Library.cs` (lines 146 and 177)

**Current:** `for (int i = 5; i * i <= n; i += 6)` recomputes `i * i` every iteration.

**Change:** Precompute `int limit = (int)Math.Sqrt(n)` before the loop, use `i <= limit` as the condition.

**Expected improvement:** ~15-20% faster for large primes due to eliminating per-iteration multiplication.

### 2c. Fix `SameDigits()` Correctness Bug

**File:** `Library.cs` (lines 222-235)

**Current:** Processes digits of `a` and `b` in lockstep within `while (a > 0)`. Two bugs: (1) If `a` has more digits than `b`, the loop still runs and `b % 10` returns 0, incorrectly decrementing `digits[0]`. (2) If `b` has more digits than `a`, the loop ends early and remaining digits of `b` are never counted (the `if (b > 0)` check catches this case but the interleaved approach is fundamentally fragile). Example: `SameDigits(102, 210)` works correctly, but `SameDigits(12, 210)` would incorrectly process `b`'s digits.

**Change:** Use two separate loops: first loop counts `a`'s digits (increment frequency), second loop counts `b`'s digits (decrement frequency), then check all frequencies are zero.

### 2d. O(n) `IsPandigital(string)` via Digit Counting

**File:** `Library.cs` (lines 201-207)

**Current:** `ToCharArray()` -> `Array.Sort()` -> `new string()` -> `.Equals("123456789")`. Three heap allocations and O(n log n) sort for 9 elements.

**Change:** Use a `Span<int>` or small `int[10]` array. Single pass: for each character, check it's '1'-'9' and increment its frequency. Then verify each digit 1-9 appears exactly once.

**Expected improvement:** O(n) vs O(n log n), zero heap allocations.

---

## Phase 3: Problem Solution Optimizations

Targets the 6 solutions with the highest speedup potential.

### 3a. Problem050 — Running Sum (10-50x potential)

**File:** `Problems/Problem050.cs` (line 32)

**Current:** `primes.Sum()` iterates the entire `Queue<int>` on every step of the sliding window. For a queue of ~500 primes, that's 500 additions per step * thousands of steps.

**Change:** Maintain a running `sum` variable. On enqueue: `sum += prime`. On dequeue: `sum -= dequeued`. Sum lookup becomes O(1) instead of O(queue.Count).

### 3b. Problem054 — Frequency-Based Hand Evaluation (5-10x potential)

**File:** `Problems/Problem054.cs`

**Current:** `HasThree()` is a triple-nested loop (O(n^3) per call). `HasFullHouse()` calls `RemoveCard()` three times, each creating a new list copy. `Wins()` calls `HasTwoPair()` twice on the same hand.

**Change:** Build a frequency map once per hand (`int[15]` indexed by card value). Detect all hand types from the frequency map:
- Pair: any frequency == 2
- Three of a kind: any frequency == 3
- Full house: has 3 AND has 2
- Two pair: count of frequencies == 2 is exactly 2

Eliminates all nested loops, list copies, and redundant analysis.

### 3c. Problem008 — Direct Character Indexing (3-5x potential)

**File:** `Problems/Problem008.cs` (line 18)

**Current:** `digitString.Substring(i, 13)` allocates a new string object every iteration (~988 iterations).

**Change:** Replace with direct character indexing: `product *= digitString[i + k] & 15` in an inner loop of length 13. Zero heap allocations in the hot loop.

### 3d. Problem004 — Descending Search with Early Exit (2-3x potential)

**File:** `Problems/Problem004.cs`

**Current:** Loops ascending from 11..999 (step 11) * 100..999, checking all ~73K combinations regardless of palindromes already found.

**Change:** Loop descending from 999 toward 100. Once a palindrome is found, set a lower bound. Inner loop breaks when `i * j < currentBest`. This lets the search terminate much earlier once large palindromes are found.

### 3e. Problem059 — Thread-Local Accumulation (2-4x potential)

**File:** `Problems/Problem059.cs` (line 34)

**Current:** `lock (lockObj)` acquired inside the innermost loop of `Parallel.For`. Every key combination (26^3 = 17,576 iterations) acquires the lock.

**Change:** Each parallel partition tracks its own `localBestScore` and `localBestSum`. After `Parallel.For` completes, find the global best from all thread-local results. Eliminates all lock contention during computation.

### 3f. Problem060 — Explicit Loops over LINQ Chains (2-3x potential)

**File:** `Problems/Problem060.cs` (lines 74-84)

**Current:** Dense LINQ query with `.Where().ToList()` at every nesting level of the 5-clique search. Each `.ToList()` materializes a new list.

**Change:** Replace LINQ chains with explicit `for` loops. Reuse list buffers where possible. Check `HashSet.Contains()` inline instead of materializing filtered lists.

---

## Files Modified (Summary)

| Phase | File | Change Type |
|-------|------|-------------|
| 1 | `BenchmarkConfig.cs` | Add WarmupRuns constant, increase progress interval |
| 1 | `Services/ProblemSolver.cs` | Add warm-up loop + GC.Collect in Run() |
| 1 | `Models/BenchmarkData.cs` | Rename TotalTime -> TotalBestTime |
| 1 | `Services/StatisticsCalculator.cs` | Update TotalTime reference |
| 1 | `Services/OutputHandler.cs` | Update TotalTime reference in reports |
| 2 | `Library.cs` | Fix GetPrimeList, IsPrime, SameDigits, IsPandigital |
| 3 | `Problems/Problem050.cs` | Running sum |
| 3 | `Problems/Problem054.cs` | Frequency-based hand evaluation |
| 3 | `Problems/Problem008.cs` | Direct character indexing |
| 3 | `Problems/Problem004.cs` | Descending search |
| 3 | `Problems/Problem059.cs` | Thread-local accumulation |
| 3 | `Problems/Problem060.cs` | Explicit loops over LINQ |

**Total: ~12 files**

---

## Verification Strategy

1. Run full benchmark BEFORE changes to establish baseline
2. After Phase 1: re-run benchmark — times should be more consistent (lower stddev)
3. After Phase 2: verify `SameDigits` bug fix with edge case, verify `GetPrimeList` produces identical output
4. After Phase 3: re-run benchmark — targeted problems should show measurable speedups
5. Verify all problem answers remain unchanged throughout
