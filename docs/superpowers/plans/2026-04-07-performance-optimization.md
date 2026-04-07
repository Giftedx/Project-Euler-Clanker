# Performance Optimization Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Make benchmark measurements accurate, fix a correctness bug in Library.cs, and speed up the 6 slowest problem solutions.

**Architecture:** Three phases executed sequentially: (1) benchmark infrastructure fixes for measurement accuracy, (2) Library.cs utility fixes/optimizations, (3) individual problem solution optimizations. Each phase is independently verifiable.

**Tech Stack:** C# / .NET 9.0, no external dependencies added.

**Spec:** `docs/superpowers/specs/2026-04-07-performance-optimization-design.md`

**Verification method:** Since there is no test project, verification is done by building (`dotnet build`) and running a full benchmark (`a` in the menu) to confirm all problem answers remain unchanged and timing improves. A baseline benchmark should be captured before any changes.

---

## File Map

| File | Action | Responsibility |
|------|--------|---------------|
| `BenchmarkConfig.cs` | Modify | Add WarmupRuns constant, increase progress interval |
| `Services/ProblemSolver.cs` | Modify | Add warm-up loop + GC.Collect in Run() |
| `Models/BenchmarkData.cs` | Modify | Rename TotalTime -> TotalBestTime |
| `Services/StatisticsCalculator.cs` | Modify | Update TotalBestTime reference |
| `Services/OutputHandler.cs` | Modify | Update TotalBestTime references (6 occurrences) |
| `Library.cs` | Modify | Fix GetPrimeList, IsPrime, SameDigits, IsPandigital |
| `Problems/Problem050.cs` | Modify | Running sum instead of Queue.Sum() |
| `Problems/Problem054.cs` | Modify | Frequency-based hand evaluation |
| `Problems/Problem008.cs` | Modify | Direct character indexing |
| `Problems/Problem004.cs` | Modify | Descending search with early exit |
| `Problems/Problem059.cs` | Modify | Thread-local accumulation |
| `Problems/Problem060.cs` | Modify | Explicit loops replacing LINQ chains |

---

## Task 0: Capture Baseline

**Files:** None modified

- [ ] **Step 1: Build the project**

```bash
cd "C:/Users/aggis/Project-Euler-Clanker"
dotnet build
```

Expected: Build succeeded.

- [ ] **Step 2: Record baseline answers**

Run the program and select `a` to run the full benchmark. Save the output `log.txt` and `benchmark.json` as baselines for comparison:

```bash
cp log.txt log-baseline.txt 2>/dev/null || true
cp benchmark.json benchmark-baseline.json 2>/dev/null || true
```

Note: If the program requires interactive input, you can verify answers after all changes by comparing `log.txt` output. The critical thing is that every problem's `Result` value stays the same.

- [ ] **Step 3: Commit baseline files**

```bash
git add log-baseline.txt benchmark-baseline.json 2>/dev/null || true
git commit -m "chore: save baseline benchmark for comparison" 2>/dev/null || true
```

---

## Task 1: Add JIT Warm-up and GC Control

**Files:**
- Modify: `BenchmarkConfig.cs:6`
- Modify: `Services/ProblemSolver.cs:65-79`

- [ ] **Step 1: Add WarmupRuns constant to BenchmarkConfig.cs**

Add the constant after `DefaultBenchmarkRuns` on line 6:

```csharp
public const int DefaultBenchmarkRuns = 100;
public const int WarmupRuns = 5;
```

- [ ] **Step 2: Add warm-up loop and GC control to ProblemSolver.Run()**

Replace the `Run` method (lines 65-80) with:

```csharp
private static ProblemData Run(int n, int runs = BenchmarkConfig.DefaultBenchmarkRuns) {
    var data = new ProblemData(n, runs);
    var problem = ProblemFactory.CreateProblem(n);

    // Warm-up: let JIT compile hot paths before timing
    for (int w = 0; w < BenchmarkConfig.WarmupRuns; w++)
        problem.Solve();

    // Clean heap state before measurement
    GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
    GC.WaitForPendingFinalizers();

    for (int i = 0; i < runs; i++) {
        var watch = Stopwatch.StartNew();

        object result = problem.Solve();
        watch.Stop();

        if (i == 0) data.Result = result.ToString() ?? string.Empty;
        data.Times.Add(watch.Elapsed.TotalMilliseconds);
    }

    return data;
}
```

- [ ] **Step 3: Build and verify**

```bash
dotnet build
```

Expected: Build succeeded.

- [ ] **Step 4: Commit**

```bash
git add BenchmarkConfig.cs Services/ProblemSolver.cs
git commit -m "perf: add JIT warm-up and GC control to benchmark runner"
```

---

## Task 2: Rename TotalTime to TotalBestTime

**Files:**
- Modify: `Models/BenchmarkData.cs:6`
- Modify: `Services/StatisticsCalculator.cs:28`
- Modify: `Services/OutputHandler.cs:40,41,60,61,113,114`

- [ ] **Step 1: Rename property in BenchmarkData.cs**

In `Models/BenchmarkData.cs`, change line 6:

```csharp
// Before:
public double TotalTime { get; set; }

// After:
public double TotalBestTime { get; set; }
```

- [ ] **Step 2: Update StatisticsCalculator.cs**

In `Services/StatisticsCalculator.cs`, change line 28:

```csharp
// Before:
benchmarkData.TotalTime += stats.Best;

// After:
benchmarkData.TotalBestTime += stats.Best;
```

- [ ] **Step 3: Update OutputHandler.cs — text report (lines 40-41)**

```csharp
// Before:
fileContent.AppendLine($"Total Time: {testData.TotalTime:F3} ms");
fileContent.AppendLine($"Average solution time: {testData.TotalTime / results.Count:F3} ms");

// After:
fileContent.AppendLine($"Total Best Time: {testData.TotalBestTime:F3} ms");
fileContent.AppendLine($"Average best time: {testData.TotalBestTime / results.Count:F3} ms");
```

- [ ] **Step 4: Update OutputHandler.cs — JSON report (lines 60-61)**

```csharp
// Before:
totalTimeMs = testData.TotalTime,
averageTimeMs = testData.TotalTime / results.Count,

// After:
totalBestTimeMs = testData.TotalBestTime,
averageBestTimeMs = testData.TotalBestTime / results.Count,
```

- [ ] **Step 5: Update OutputHandler.cs — HTML report (lines 113-114)**

```csharp
// Before:
totalTimeMs = testData.TotalTime,
averageTimeMs = testData.TotalTime / results.Count,

// After:
totalBestTimeMs = testData.TotalBestTime,
averageBestTimeMs = testData.TotalBestTime / results.Count,
```

- [ ] **Step 6: Build and verify**

```bash
dotnet build
```

Expected: Build succeeded. No remaining references to `TotalTime`.

- [ ] **Step 7: Commit**

```bash
git add Models/BenchmarkData.cs Services/StatisticsCalculator.cs Services/OutputHandler.cs
git commit -m "refactor: rename TotalTime to TotalBestTime for accuracy"
```

---

## Task 3: Reduce Progress Thread Polling Interval

**Files:**
- Modify: `BenchmarkConfig.cs:8`

- [ ] **Step 1: Change the interval**

In `BenchmarkConfig.cs`, change line 8:

```csharp
// Before:
public const int ProgressUpdateIntervalMs = 10;

// After:
public const int ProgressUpdateIntervalMs = 100;
```

- [ ] **Step 2: Build and verify**

```bash
dotnet build
```

Expected: Build succeeded.

- [ ] **Step 3: Commit**

```bash
git add BenchmarkConfig.cs
git commit -m "perf: reduce progress bar polling from 10ms to 100ms"
```

---

## Task 4: Fix GetPrimeList to Use Sieve

**Files:**
- Modify: `Library.cs:280-283`

- [ ] **Step 1: Replace GetPrimeList implementation**

Replace lines 280-283 in `Library.cs`:

```csharp
// Before:
public static void GetPrimeList(int upperLimit, out List<int> primeList) {
    var numbers = Enumerable.Range(1, upperLimit).ToList();
    primeList = numbers.AsParallel().Where(IsPrime).ToList();
}

// After:
public static void GetPrimeList(int upperLimit, out List<int> primeList) {
    bool[] sieve = GetSieve(upperLimit);
    primeList = new List<int>();
    for (int i = 2; i < upperLimit; i++) {
        if (sieve[i]) primeList.Add(i);
    }
}
```

- [ ] **Step 2: Build and verify**

```bash
dotnet build
```

Expected: Build succeeded.

- [ ] **Step 3: Commit**

```bash
git add Library.cs
git commit -m "perf: replace trial-division GetPrimeList with sieve-based collection"
```

---

## Task 5: Optimize IsPrime Loop Condition

**Files:**
- Modify: `Library.cs:145-150` (int version) and `Library.cs:176-181` (long version)

- [ ] **Step 1: Optimize the int IsPrime loop**

Replace the loop in the `default` branch of `IsPrime(int n)` (around lines 141-153):

```csharp
// Before:
default:
    if (n % 2 == 0 || n % 3 == 0) {
        result = false;
    } else {
        result = true;
        for (int i = 5; i * i <= n; i += 6) {
            if (n % i != 0 && n % (i + 2) != 0) continue;
            result = false;
            break;
        }
    }

    break;

// After:
default:
    if (n % 2 == 0 || n % 3 == 0) {
        result = false;
    } else {
        result = true;
        int limit = (int)Math.Sqrt(n);
        for (int i = 5; i <= limit; i += 6) {
            if (n % i == 0 || n % (i + 2) == 0) {
                result = false;
                break;
            }
        }
    }

    break;
```

- [ ] **Step 2: Optimize the long IsPrime loop**

Replace the loop in the `default` branch of `IsPrime(long n)` (around lines 172-185):

```csharp
// Before:
default:
    if (n % 2 == 0 || n % 3 == 0) {
        result = false;
    } else {
        result = true;
        for (long i = 5; i * i <= n; i += 6) {
            if (n % i != 0 && n % (i + 2) != 0) continue;
            result = false;
            break;
        }
    }

    break;

// After:
default:
    if (n % 2 == 0 || n % 3 == 0) {
        result = false;
    } else {
        result = true;
        long limit = (long)Math.Sqrt(n);
        for (long i = 5; i <= limit; i += 6) {
            if (n % i == 0 || n % (i + 2) == 0) {
                result = false;
                break;
            }
        }
    }

    break;
```

- [ ] **Step 3: Build and verify**

```bash
dotnet build
```

Expected: Build succeeded.

- [ ] **Step 4: Commit**

```bash
git add Library.cs
git commit -m "perf: precompute sqrt limit in IsPrime loops"
```

---

## Task 6: Fix SameDigits Correctness Bug

**Files:**
- Modify: `Library.cs:222-235`

- [ ] **Step 1: Replace SameDigits with correct implementation**

Replace lines 222-235 in `Library.cs`:

```csharp
// Before:
public static bool SameDigits(int a, int b) {
    int[] digits = new int[10];
    while (a > 0) {
        int digita = a % 10;
        int digitb =  b % 10;
        a /= 10;
        b /= 10;
        digits[digita]++;
        digits[digitb]--;
    }
    if(b > 0)return false;
    foreach (int digit in digits) if(digit != 0)return false;
    return true;
}

// After:
public static bool SameDigits(int a, int b) {
    int[] digits = new int[10];
    while (a > 0) {
        digits[a % 10]++;
        a /= 10;
    }
    while (b > 0) {
        digits[b % 10]--;
        b /= 10;
    }
    foreach (int count in digits) {
        if (count != 0) return false;
    }
    return true;
}
```

- [ ] **Step 2: Build and verify**

```bash
dotnet build
```

Expected: Build succeeded.

- [ ] **Step 3: Commit**

```bash
git add Library.cs
git commit -m "fix: correct SameDigits to handle different digit counts"
```

---

## Task 7: Optimize IsPandigital(string)

**Files:**
- Modify: `Library.cs:201-207`

- [ ] **Step 1: Replace with O(n) digit-counting implementation**

Replace lines 201-207 in `Library.cs`:

```csharp
// Before:
public static bool IsPandigital(string s) {
    if (s.Length != 9) return false;
    char[] chars = s.ToCharArray();
    Array.Sort(chars);
    bool result = new string(chars).Equals("123456789");
    return result;
}

// After:
public static bool IsPandigital(string s) {
    if (s.Length != 9) return false;
    int[] digits = new int[10];
    foreach (char c in s) {
        if (c < '1' || c > '9') return false;
        digits[c - '0']++;
    }
    for (int i = 1; i <= 9; i++) {
        if (digits[i] != 1) return false;
    }
    return true;
}
```

- [ ] **Step 2: Build and verify**

```bash
dotnet build
```

Expected: Build succeeded.

- [ ] **Step 3: Commit**

```bash
git add Library.cs
git commit -m "perf: O(n) IsPandigital via digit counting instead of sort"
```

---

## Task 8: Optimize Problem050 — Running Sum

**Files:**
- Modify: `Problems/Problem050.cs`

- [ ] **Step 1: Replace the entire Problem050 class**

Replace the full contents of `Problems/Problem050.cs`:

```csharp
namespace Project_Euler.Problems;

public class Problem050 : Problem {
    private const int Limit = 1000000;
    private readonly bool[] _isPrime = Library.GetSieve(Limit);

    public override object Solve() {
        return ConsecutivePrimeSumBelow(Limit);
    }

    private long ConsecutivePrimeSumBelow(int n) {
        // Build list of primes up to n
        var primes = new List<int>();
        for (int i = 2; i < n; i++) {
            if (_isPrime[i]) primes.Add(i);
        }

        // Build prefix sums for O(1) range sum queries
        long[] prefixSum = new long[primes.Count + 1];
        for (int i = 0; i < primes.Count; i++) {
            prefixSum[i + 1] = prefixSum[i] + primes[i];
        }

        int bestLength = 0;
        long bestPrime = 0;

        for (int start = 0; start < primes.Count; start++) {
            for (int end = start + bestLength + 1; end <= primes.Count; end++) {
                long sum = prefixSum[end] - prefixSum[start];
                if (sum >= n) break;
                if (sum > 0 && sum < _isPrime.Length && _isPrime[(int)sum]) {
                    int length = end - start;
                    if (length > bestLength) {
                        bestLength = length;
                        bestPrime = sum;
                    }
                }
            }
        }

        return bestPrime;
    }
}
```

- [ ] **Step 2: Build and verify**

```bash
dotnet build
```

Expected: Build succeeded.

- [ ] **Step 3: Commit**

```bash
git add Problems/Problem050.cs
git commit -m "perf: Problem050 use prefix sums for O(1) range queries"
```

---

## Task 9: Optimize Problem054 — Frequency-Based Hand Evaluation

**Files:**
- Modify: `Problems/Problem054.cs`

- [ ] **Step 1: Replace the entire Problem054 class**

Replace the full contents of `Problems/Problem054.cs`:

```csharp
namespace Project_Euler.Problems;

public class Problem054 : Problem {
    private readonly string[] _hands = File.ReadAllLines("poker.txt");

    public override object Solve() {
        return Player1WinCount();
    }

    private int Player1WinCount() {
        int totalWins = 0;
        foreach (string line in _hands) {
            string[] cards = line.Split(' ');
            var hand1 = ParseHand(cards, 0);
            var hand2 = ParseHand(cards, 5);

            int rank1 = RankHand(hand1);
            int rank2 = RankHand(hand2);

            if (rank1 > rank2) totalWins++;
        }

        return totalWins;
    }

    private static (int value, char suit)[] ParseHand(string[] cards, int offset) {
        var hand = new (int value, char suit)[5];
        for (int i = 0; i < 5; i++) {
            string card = cards[offset + i];
            hand[i] = (ParseValue(card[0]), card[1]);
        }
        return hand;
    }

    private static int ParseValue(char c) => c switch {
        'T' => 10, 'J' => 11, 'Q' => 12, 'K' => 13, 'A' => 14,
        _ => c - '0'
    };

    // Returns a comparable rank score. Higher = better hand.
    // Format: category (0-9) in high bits, then tiebreaker values.
    private static int RankHand((int value, char suit)[] hand) {
        // Build frequency map
        int[] freq = new int[15];
        foreach (var (value, _) in hand) freq[value]++;

        // Identify groups
        int pairs = 0, pairHigh = 0, pairLow = 0;
        int threeVal = 0, fourVal = 0;

        for (int v = 14; v >= 2; v--) {
            switch (freq[v]) {
                case 4: fourVal = v; break;
                case 3: threeVal = v; break;
                case 2:
                    pairs++;
                    if (pairs == 1) pairHigh = v;
                    else pairLow = v;
                    break;
            }
        }

        // Check flush
        bool isFlush = hand[0].suit == hand[1].suit &&
                       hand[1].suit == hand[2].suit &&
                       hand[2].suit == hand[3].suit &&
                       hand[3].suit == hand[4].suit;

        // Check straight
        int[] values = new int[5];
        for (int i = 0; i < 5; i++) values[i] = hand[i].value;
        Array.Sort(values);
        bool isStraight = values[4] - values[0] == 4 &&
                          freq[values[0]] == 1 && freq[values[1]] == 1 &&
                          freq[values[2]] == 1 && freq[values[3]] == 1 &&
                          freq[values[4]] == 1;

        int highCard = values[4];

        // Build kickers list (cards not in groups, descending)
        int kickers = 0;
        int shift = 0;
        for (int v = 2; v <= 14; v++) {
            if (freq[v] == 1 && v != threeVal && v != fourVal) {
                kickers |= v << (shift * 4);
                shift++;
            }
        }

        // Rank by category
        if (isStraight && isFlush)       return (8 << 20) | highCard;
        if (fourVal > 0)                 return (7 << 20) | (fourVal << 4) | kickers;
        if (threeVal > 0 && pairs >= 1)  return (6 << 20) | (threeVal << 4) | pairHigh;
        if (isFlush)                     return (5 << 20) | kickers;
        if (isStraight)                  return (4 << 20) | highCard;
        if (threeVal > 0)               return (3 << 20) | (threeVal << 8) | kickers;
        if (pairs == 2)                  return (2 << 20) | (pairHigh << 8) | (pairLow << 4) | kickers;
        if (pairs == 1)                  return (1 << 20) | (pairHigh << 8) | kickers;
        return kickers;
    }
}
```

- [ ] **Step 2: Build and verify**

```bash
dotnet build
```

Expected: Build succeeded.

- [ ] **Step 3: Commit**

```bash
git add Problems/Problem054.cs
git commit -m "perf: Problem054 frequency-based hand evaluation"
```

---

## Task 10: Optimize Problem008 — Direct Character Indexing

**Files:**
- Modify: `Problems/Problem008.cs:13-24`

- [ ] **Step 1: Replace LargestProductString method**

Replace the `LargestProductString` method (lines 13-24) in `Problems/Problem008.cs`:

```csharp
// Before:
private long LargestProductString(int len, string digitString) {
    int numberBound = digitString.Length - (len + 1);
    long highest = 0;
    for (int i = 0; i < numberBound; i++) {
        long product = 1;
        string strNum = digitString.Substring(i, 13);
        foreach (char c in strNum) product *= c & 15;
        if (product > highest) highest = product;
    }

    return highest;
}

// After:
private long LargestProductString(int len, string digitString) {
    long highest = 0;
    for (int i = 0; i <= digitString.Length - len; i++) {
        long product = 1;
        for (int k = 0; k < len; k++) {
            product *= digitString[i + k] & 15;
        }
        if (product > highest) highest = product;
    }

    return highest;
}
```

- [ ] **Step 2: Build and verify**

```bash
dotnet build
```

Expected: Build succeeded.

- [ ] **Step 3: Commit**

```bash
git add Problems/Problem008.cs
git commit -m "perf: Problem008 direct character indexing, no substring allocation"
```

---

## Task 11: Optimize Problem004 — Descending Search with Early Exit

**Files:**
- Modify: `Problems/Problem004.cs`

- [ ] **Step 1: Replace the full Problem004 class**

Replace the full contents of `Problems/Problem004.cs`:

```csharp
namespace Project_Euler.Problems;

public class Problem004 : Problem {
    public override object Solve() {
        return LargestPalindromeProduct();
    }

    private int LargestPalindromeProduct() {
        int best = 0;
        for (int i = 999; i >= 100; i--) {
            if (i * 999 <= best) break; // No possible product can beat best
            // 6-digit palindrome is divisible by 11, so one factor must be a multiple of 11
            int jStart = (i % 11 == 0) ? 999 : 999 - (999 % 11);
            int jStep = (i % 11 == 0) ? 1 : 11;
            for (int j = jStart; j >= i; j -= jStep) {
                int product = i * j;
                if (product <= best) break;
                if (Library.IsPalindrome(product))
                    best = product;
            }
        }

        return best;
    }
}
```

- [ ] **Step 2: Build and verify**

```bash
dotnet build
```

Expected: Build succeeded.

- [ ] **Step 3: Commit**

```bash
git add Problems/Problem004.cs
git commit -m "perf: Problem004 descending search with early exit"
```

---

## Task 12: Optimize Problem059 — Thread-Local Accumulation

**Files:**
- Modify: `Problems/Problem059.cs`

- [ ] **Step 1: Replace the full Problem059 class**

Replace the full contents of `Problems/Problem059.cs`:

```csharp
namespace Project_Euler.Problems;

public class Problem059 : Problem {
    private readonly int[] _ciphertext;

    public Problem059() {
        string input = File.ReadAllText("0059_cipher.txt");
        _ciphertext = input.Split(',').Select(int.Parse).ToArray();
    }

    public override object Solve() {
        return SumAscii();
    }

    private int SumAscii() {
        int len = _ciphertext.Length;
        int bestSum = 0;
        int maxScore = int.MinValue;

        Parallel.For(97, 123, () => (score: int.MinValue, sum: 0),
            (a, _, localBest) => {
                for (int b = 97; b <= 122; b++) {
                    for (int c = 97; c <= 122; c++) {
                        int score = 0;
                        int sum = 0;

                        for (int i = 0; i < len; i++) {
                            int key = (i % 3) switch {
                                0 => a,
                                1 => b,
                                _ => c
                            };
                            int decoded = _ciphertext[i] ^ key;
                            score += ScoreChar(decoded);
                            sum += decoded;
                        }

                        if (score > localBest.score) {
                            localBest = (score, sum);
                        }
                    }
                }
                return localBest;
            },
            localBest => {
                lock (_ciphertext) {
                    if (localBest.score > maxScore) {
                        maxScore = localBest.score;
                        bestSum = localBest.sum;
                    }
                }
            });

        return bestSum;

        static int ScoreChar(int c) => c switch {
            >= 'a' and <= 'z' => 2,
            >= 'A' and <= 'Z' => 1,
            ' ' => 3,
            '.' or ',' or '\'' or ';' => 1,
            _ => -1
        };
    }
}
```

- [ ] **Step 2: Build and verify**

```bash
dotnet build
```

Expected: Build succeeded.

- [ ] **Step 3: Commit**

```bash
git add Problems/Problem059.cs
git commit -m "perf: Problem059 thread-local accumulation, no lock contention"
```

---

## Task 13: Optimize Problem060 — Explicit Loops Replacing LINQ

**Files:**
- Modify: `Problems/Problem060.cs:65-87`

- [ ] **Step 1: Replace the clique search section**

In `Problems/Problem060.cs`, replace the clique search portion starting from `var candidates` (line 65) through the end of the `Parallel.ForEach` (line 87):

```csharp
// Before (lines 65-87):
var candidates = primes.Where(p => graph.ContainsKey(p) && graph[p].Count >= 4).ToList();

int minSum = int.MaxValue;
object lockObj = new();

Parallel.ForEach(candidates, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, a => {
    var aList = graph[a].Where(x => x > a).ToList();
    if (aList.Count < 4) return;

    foreach (int sum in from b in aList
             where graph.ContainsKey(b)
             let bList = graph[b]
             let commonAB = aList.Where(c => c > b && bList.Contains(c)).ToList()
             where commonAB.Count >= 3 from c in commonAB
             where graph.ContainsKey(c) let cList = graph[c]
             let commonABC = commonAB.Where(d => d > c && bList.Contains(d) && cList.Contains(d)).ToList()
             where commonABC.Count >= 2 from d in commonABC
             where graph.ContainsKey(d) let dList = graph[d] from e
                 in commonABC.Where(e => e > d && bList.Contains(e) && cList.Contains(e) && dList.Contains(e))
             select a + b + c + d + e) {
        lock (lockObj) if (sum < minSum) minSum = sum;
    }
});

// After:
var candidates = new List<int>();
foreach (int p in primes) {
    if (graph.TryGetValue(p, out var neighbors) && neighbors.Count >= 4)
        candidates.Add(p);
}

int minSum = int.MaxValue;

Parallel.ForEach(candidates, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
    () => int.MaxValue,
    (a, _, localMin) => {
        if (!graph.TryGetValue(a, out var aNeighbors)) return localMin;

        var aList = new List<int>();
        foreach (int x in aNeighbors) {
            if (x > a) aList.Add(x);
        }
        if (aList.Count < 4) return localMin;

        for (int bi = 0; bi < aList.Count; bi++) {
            int b = aList[bi];
            if (!graph.TryGetValue(b, out var bNeighbors)) continue;

            var commonAB = new List<int>();
            for (int ci = bi + 1; ci < aList.Count; ci++) {
                if (bNeighbors.Contains(aList[ci]))
                    commonAB.Add(aList[ci]);
            }
            if (commonAB.Count < 3) continue;

            for (int ci = 0; ci < commonAB.Count; ci++) {
                int c = commonAB[ci];
                if (!graph.TryGetValue(c, out var cNeighbors)) continue;

                var commonABC = new List<int>();
                for (int di = ci + 1; di < commonAB.Count; di++) {
                    int d = commonAB[di];
                    if (bNeighbors.Contains(d) && cNeighbors.Contains(d))
                        commonABC.Add(d);
                }
                if (commonABC.Count < 2) continue;

                for (int di = 0; di < commonABC.Count; di++) {
                    int d = commonABC[di];
                    if (!graph.TryGetValue(d, out var dNeighbors)) continue;

                    for (int ei = di + 1; ei < commonABC.Count; ei++) {
                        int e = commonABC[ei];
                        if (bNeighbors.Contains(e) && cNeighbors.Contains(e) && dNeighbors.Contains(e)) {
                            int sum = a + b + c + d + e;
                            if (sum < localMin) localMin = sum;
                        }
                    }
                }
            }
        }

        return localMin;
    },
    localMin => {
        if (localMin < int.MaxValue) {
            int current = Volatile.Read(ref minSum);
            while (localMin < current) {
                int prev = Interlocked.CompareExchange(ref minSum, localMin, current);
                if (prev == current) break;
                current = prev;
            }
        }
    });
```

- [ ] **Step 2: Build and verify**

```bash
dotnet build
```

Expected: Build succeeded.

- [ ] **Step 3: Commit**

```bash
git add Problems/Problem060.cs
git commit -m "perf: Problem060 explicit loops and thread-local min instead of LINQ + lock"
```

---

## Task 14: Final Verification

**Files:** None modified

- [ ] **Step 1: Full build**

```bash
dotnet build
```

Expected: Build succeeded with 0 errors, 0 warnings (or same warnings as baseline).

- [ ] **Step 2: Run full benchmark and compare answers**

Run the program and select `a` to benchmark all problems. Compare every problem's `Result` value against the baseline `log-baseline.txt` to confirm no answers changed.

```bash
# After running the benchmark:
diff <(grep "Problem " log-baseline.txt | head -60) <(grep "Problem " log.txt | head -60) || echo "Answers differ - investigate!"
```

Expected: All problem answers are identical. Timing values should show improvement, especially for Problems 004, 008, 050, 054, 059, and 060.

- [ ] **Step 3: Commit verification notes**

```bash
git add log.txt benchmark.json 2>/dev/null || true
git commit -m "chore: post-optimization benchmark results" 2>/dev/null || true
```
