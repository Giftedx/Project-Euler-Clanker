using System.Numerics;
using System.Runtime.CompilerServices;

namespace Project_Euler.Problems;

public class Problem014 : Problem {
    private static uint _threshold;
    
    public override object Solve() {
        return FindBestCandidate(1000000u);
    }

    private static unsafe uint FindBestCandidate(uint maxLimit) {
        _threshold = (uint)(maxLimit * 0.075) + 2;
        
        ushort* lookupTable = stackalloc ushort[(int)_threshold + 3];
        GenerateOptimizedLookupTable(lookupTable);
        
        ulong* skipBits = stackalloc ulong[(int)((maxLimit + 63) / 64)];
        GenerateSkipBitset(skipBits, maxLimit);
        
        uint* results = stackalloc uint[32];
        
        Parallel.For(0, 16, threadIndex => {
            (uint bestNum, uint bestScore) = ProcessCandidatesUltraFast(threadIndex, maxLimit, lookupTable, skipBits);
            results[threadIndex * 2] = bestNum;
            results[threadIndex * 2 + 1] = bestScore;
        });
        
        return FindOptimalResult(results, maxLimit, lookupTable);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static unsafe void GenerateSkipBitset(ulong* skipBits, uint maxLimit) {
        uint numWords = (maxLimit + 63) / 64;
        for (uint i = 0; i < numWords; i++) skipBits[i] = 0;
        
        for (uint n = 4; n < maxLimit; n += 9) {
            uint wordIndex = n / 64;
            uint bitIndex = n % 64;
            skipBits[wordIndex] |= 1UL << (int)bitIndex;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static unsafe bool ShouldSkip(uint candidate, ulong* skipBits) {
        uint wordIndex = candidate / 64;
        uint bitIndex = candidate % 64;
        return (skipBits[wordIndex] & (1UL << (int)bitIndex)) != 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static unsafe (uint bestNum, uint bestScore) ProcessCandidatesUltraFast(
        int threadIndex, uint maxLimit, ushort* lookupTable, ulong* skipBits) {
        
        uint start = (maxLimit >> 1) | 1;
        const uint step = 48;
        uint offset = (uint)(threadIndex * 3);
        uint candidate = start + offset;
        
        candidate += candidate % 6 == 1 || candidate % 6 == 5 ? 0u : 1u;
        
        uint bestNum = 0;
        uint bestScore = 0;
        
        for (; candidate + step * 3 < maxLimit; candidate += step * 4) {
            uint c2 = candidate + step;
            uint c3 = candidate + step * 2;
            uint c4 = candidate + step * 3;
            
            if (((candidate + 3) & 7) == 0 || ShouldSkip(candidate, skipBits)) goto skip1;
            if (((c2 + 3) & 7) == 0 || ShouldSkip(c2, skipBits)) goto skip2;
            if (((c3 + 3) & 7) == 0 || ShouldSkip(c3, skipBits)) goto skip3;
            if (((c4 + 3) & 7) == 0 || ShouldSkip(c4, skipBits)) goto skip4;
            
            uint s1 = ComputeScoreUltraFast(candidate, lookupTable);
            if (s1 > bestScore) { bestScore = s1; bestNum = candidate; }
            
            skip1:
            uint s2 = ComputeScoreUltraFast(c2, lookupTable);
            if (s2 > bestScore) { bestScore = s2; bestNum = c2; }
            
            skip2:
            uint s3 = ComputeScoreUltraFast(c3, lookupTable);
            if (s3 > bestScore) { bestScore = s3; bestNum = c3; }
            
            skip3:
            uint s4 = ComputeScoreUltraFast(c4, lookupTable);
            if (s4 > bestScore) { bestScore = s4; bestNum = c4; }
            
            skip4:;
        }
        
        for (; candidate < maxLimit; candidate += step) {
            if (((candidate + 3) & 7) == 0 || ShouldSkip(candidate, skipBits)) continue;
            
            uint score = ComputeScoreUltraFast(candidate, lookupTable);
            if (score <= bestScore) continue;
            bestScore = score;
            bestNum = candidate;
        }
        
        return (bestNum, bestScore);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static unsafe uint ComputeScoreUltraFast(uint number, ushort* lookupTable) {
        ulong n = number;

        n = (n * 3 + 1) >> 1;
        uint totalSteps = 2;

        while (n >= _threshold) {
            if ((n & 1) == 0) {
                uint zeros = (uint)BitOperations.TrailingZeroCount(n);
                n >>= (int)zeros;
                totalSteps += zeros;
            } else {
                n = (n * 3 + 1) >> 1;
                totalSteps += 2;
            }
        }
        
        return totalSteps + lookupTable[n];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void GenerateOptimizedLookupTable(ushort* table) {
        table[1] = 0;
        table[2] = 1;
        table[3] = 7;
        
        uint seqIndex = 4;
        uint jumpIndex = 4;

        while (seqIndex < _threshold) {
            if (seqIndex < _threshold) {
                table[seqIndex] = (ushort)(table[seqIndex >> 1] + 1);
                seqIndex++;
            }
            
            if (seqIndex < _threshold && jumpIndex < seqIndex) {
                table[seqIndex] = (ushort)(table[jumpIndex] + 3);
                jumpIndex += 3;
                seqIndex++;
            }
            
            if (seqIndex < _threshold) {
                table[seqIndex] = (ushort)(table[seqIndex >> 1] + 1);
                seqIndex++;
            }

            if (seqIndex >= _threshold) continue;
            table[seqIndex] = ComputeTableEntryFast(seqIndex, table);
            seqIndex++;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe ushort ComputeTableEntryFast(uint n, ushort* table) {
        ulong num = n;
        uint steps = 0;
        
        while (num >= n) {
            if ((num & 1) == 0) {
                uint zeros = (uint)BitOperations.TrailingZeroCount(num);
                num >>= (int)zeros;
                steps += zeros;
            } else {
                num = (num * 3 + 1) >> 1;
                steps += 2;
            }
        }
        
        return (ushort)(steps + table[num]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe uint FindOptimalResult(uint* results, uint maxLimit, ushort* lookupTable) {
        uint maxScore = 0;
        uint bestNumber = 0;
        
        for (int i = 0; i < 16; i++) {
            uint num = results[i * 2];
            uint score = results[i * 2 + 1];

            if (score <= maxScore && (score != maxScore || num >= bestNumber)) continue;
            maxScore = score;
            bestNumber = num;
        }
        
        uint doubled = bestNumber * 2;
        if (doubled >= maxLimit) return bestNumber;
        uint doubledScore = ComputeScoreUltraFast(doubled, lookupTable);
        return doubledScore > maxScore ? doubled : bestNumber;
    }
}