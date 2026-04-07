namespace Project_Euler.Problems;

public class Problem035 : Problem {
    private readonly bool[] _sieve = Library.GetSieve(1_000_000);

    public override object Solve() {
        return CircularPrimeCount();
    }

    private int CircularPrimeCount() {
        int count = 4; // 2, 3, 5, 7

        for (int a = 1; a <= 9; a += 2) {
            if (a == 5) continue;
            for (int b = a; b <= 9; b += 2) {
                if (b == 5) continue;
                if (_sieve[a * 10 + b] && _sieve[b * 10 + a])
                    count += a != b ? 2 : 1;
                int startC = a == b ? a : a + 2;
                for (int c = startC; c <= 9; c += 2) {
                    if (c == 5) continue;
                    if (_sieve[Compose(a, b, c)] &&
                        _sieve[Compose(b, c, a)] &&
                        _sieve[Compose(c, a, b)])
                        count += a != c ? 3 : 1;

                    int startD = a == b && a == c ? a : a + 2;
                    for (int d = startD; d <= 9; d += 2) {
                        if (d == 5) continue;
                        if (_sieve[Compose(a, b, c, d)] &&
                            _sieve[Compose(b, c, d, a)] &&
                            _sieve[Compose(c, d, a, b)] &&
                            _sieve[Compose(d, a, b, c)])
                            count += 4;

                        int startE = a == b && a == c && a == d ? a : a + 2;
                        for (int e = startE; e <= 9; e += 2) {
                            if (e == 5) continue;
                            if (_sieve[Compose(a, b, c, d, e)] &&
                                _sieve[Compose(b, c, d, e, a)] &&
                                _sieve[Compose(c, d, e, a, b)] &&
                                _sieve[Compose(d, e, a, b, c)] &&
                                _sieve[Compose(e, a, b, c, d)])
                                count += a != e ? 5 : 1;

                            for (int f = a + 2; f <= 9; f += 2) {
                                if (f == 5) continue;
                                if (_sieve[Compose(a, b, c, d, e, f)] &&
                                    _sieve[Compose(b, c, d, e, f, a)] &&
                                    _sieve[Compose(c, d, e, f, a, b)] &&
                                    _sieve[Compose(d, e, f, a, b, c)] &&
                                    _sieve[Compose(e, f, a, b, c, d)] &&
                                    _sieve[Compose(f, a, b, c, d, e)])
                                    count += 6;
                            }
                        }
                    }
                }
            }
        }

        return count;
    }

    private static int Compose(int a, int b, int c) => a * 100 + b * 10 + c;
    private static int Compose(int a, int b, int c, int d) => a * 1000 + b * 100 + c * 10 + d;
    private static int Compose(int a, int b, int c, int d, int e) => a * 10000 + b * 1000 + c * 100 + d * 10 + e;
    private static int Compose(int a, int b, int c, int d, int e, int f) => a * 100000 + b * 10000 + c * 1000 + d * 100 + e * 10 + f;
}
