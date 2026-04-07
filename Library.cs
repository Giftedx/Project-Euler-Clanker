 using System.Collections;
 using System.Collections.Concurrent;
 using System.Numerics;

// ReSharper disable UnusedMember.Global

namespace Project_Euler;

public static class Library {
    private static readonly ConcurrentDictionary<int, bool> _primeCache = new();
    private static readonly ConcurrentDictionary<long, bool> _primeCacheLong = new();
    
    //Program-wide tasks.
    public static void FunPrint(string s) {
        const int wait = 100;
        foreach (char c in s) {
            Console.Write(c);
            Thread.Sleep(Random.Shared.Next(wait));
        }
        Console.WriteLine();
    }

    public static void ReadFile(string fileName, out List<string> data) {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        data = File.ReadAllText(filePath)
                   .Replace("\"", "")
                   .Split(',')
                   .ToList();
    }

    //Maths tasks.

    public static int SumDigits(BigInteger digits) {
        BigInteger sum = 0;
        while (digits != 0) {
            var last = digits % 10;
            sum += last;
            digits /= 10;
        }

        return (int)sum;
    }

    public static int Gcd(int a, int b) {
        while (b != 0) {
            int temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    public static BigInteger Factorial(int n) {
        BigInteger factorial = 1;
        for (int i = 2; i <= n; i++) factorial *= i;
        return factorial;
    }

    public static int IntFactorial(int n) {
        int factorial = 1;
        for (int i = 2; i <= n; i++) factorial *= i;
        return factorial;
    }

    public static int Pow10(int exp) {
        int res = 1;
        while (exp-- > 0) res *= 10;
        return res;
    }

    public static int DigitCount(int n) {
        int count = 0;
        while (n > 0) {
            count++;
            n /= 10;
        }

        return count;
    }
    
    public static int NumDigits(int n) {
        return n switch {
            < 10 => 1,
            < 100 => 2,
            < 1000 => 3,
            < 10000 => 4,
            < 100000 => 5,
            < 1000000 => 6,
            < 10000000 => 7,
            < 100000000 => 8,
            < 1000000000 => 9,
            _ => 10
        };
    }

    //Boolean Checks

    public static bool IsPalindrome(int n) {
        int reverse = 0;
        int temp = Math.Abs(n);
        while (temp != 0) {
            reverse = reverse * 10 + temp % 10;
            temp /= 10;
        }

        return reverse == Math.Abs(n);
    }
    
    public static bool IsPalindrome(long n) {
        long reverse = 0;
        long temp = Math.Abs(n);
        while (temp != 0) {
            reverse = reverse * 10 + temp % 10;
            temp /= 10;
        }

        return reverse == Math.Abs(n);
    }

    public static bool IsPalindrome(string s) {
        for (int i = 0; i < s.Length / 2; i++)
            if (s[i] != s[s.Length - 1 - i])
                return false;

        return true;
    }
    
    public static bool IsPrime(int n) {
        if (_primeCache.TryGetValue(n, out bool cached))
            return cached;

        bool result;
        switch (n) {
            case <= 1:
                result = false;
                break;
            case 2 or 3:
                result = true;
                break;
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
        }

        _primeCache[n] = result;
        return result;
    }

    public static bool IsPrime(long n) {
        if (_primeCacheLong.TryGetValue(n, out bool cached))
            return cached;

        bool result;
        switch (n) {
            case <= 1:
                result = false;
                break;
            case 2 or 3:
                result = true;
                break;
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
        }

        _primeCacheLong[n] = result;
        return result;
    }

    public static void PrecomputePrimes(int limit) {
        bool[] sieve = GetSieve(limit);
        for (int i = 2; i < limit; i++) _primeCache[i] = sieve[i];
    }

    public static bool IsPentagon(long pn) {
        double n = 1.0 / 6.0 * (Math.Sqrt(24 * pn + 1) + 1);
        return n - (int)n == 0;
    }

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

    public static bool IsPanDigital(int n) {
        //test this out
        int result = 0;
        while (n > 0) {
            int digit = n % 10;
            if (digit == 0) return false;
            result |= 1 << (digit - 1);
            n /= 10;
        }

        return result == 0x1ff;
    }
    
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

    //Array Operations

    public static bool Permute(int[] arr) {
        int i = arr.Length - 1;
        while (i > 0 && arr[i - 1] >= arr[i]) i--;
        if (i <= 0) return false;
        int j = arr.Length - 1;
        while (arr[j] <= arr[i - 1]) j--;
        (arr[i - 1], arr[j]) = (arr[j], arr[i - 1]);
        for (int k = arr.Length - 1; i < k; i++, k--)
            (arr[i], arr[k]) = (arr[k], arr[i]);
        return true;
    }

    public static bool[] GetSieve(int limit) {
        SieveOfEratosthenes(limit, out bool[] sieve);
        return sieve;
    }

    public static void SieveOfEratosthenes(int n, out bool[] isPrime) {
        isPrime = new bool[n];
        if (n <= 2) return;
        Array.Fill(isPrime, true, 2, n - 2);
        int limit = (int)Math.Sqrt(n) + 1;
        for (int i = 2; i < limit; i++) {
            if (!isPrime[i]) continue;
            for (int j = i * i; j < n; j += i)
                isPrime[j] = false;
        }
    }

    public static void SieveOfEratosthenes(int n, out BitArray isPrime) {
        isPrime = new BitArray(n, false);
        if (n < 2) return;
        isPrime[2] = true;
        for (int i = 3; i < n; i += 2) isPrime[i] = true;
        int limit = (int)Math.Sqrt(n) + 1;
        for (int i = 3; i <= limit; i += 2) {
            if (!isPrime[i]) continue;
            for (int j = i * i; j < n; j += 2 * i) isPrime[j] = false;
        }
    }

    public static void GetPrimeList(int upperLimit, out List<int> primeList) {
        bool[] sieve = GetSieve(upperLimit);
        primeList = new List<int>();
        for (int i = 2; i < upperLimit; i++) {
            if (sieve[i]) primeList.Add(i);
        }
    }
}