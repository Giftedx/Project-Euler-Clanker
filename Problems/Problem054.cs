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
