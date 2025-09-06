namespace Project_Euler.Problems;

public class Problem054 :  Problem {
    private readonly string[] _hands = File.ReadAllLines("poker.txt");

    public override object Solve() {
	    return Player1WinCount();
    }

    private int Player1WinCount() {
        int totalWins = 0;
        foreach (string line in _hands) {
            string[] cards = line.Split(' ');
            var hand1 = cards.Take(5).Select(c => new Card(c)).ToList();
            var hand2 = cards.Skip(5).Take(5).Select(c => new Card(c)).ToList();

            if (Wins(hand1, hand2)) totalWins++;
        }

        return totalWins;
    }

    private bool Wins(List<Card> hand1, List<Card> hand2) {
        (int a, int b) = HasTwoPair(hand1);
        (int c, int d) = HasTwoPair(hand2);

        if ((a != 0 && a == b) || (c != 0 && d == c)) {
            if (a != 0 && c == 0) return true;
            if (c != 0 && a == 0) return false;
        }

        (a, b) = HasFullHouse(hand1);
        (c, d) = HasFullHouse(hand2);

        if (a + b + c + d != 0) {
            if (a != 0 && c == 0) return true;
            if (c != 0 && a == 0) return false;
        }

        if (HasFlush(hand1) && !HasFlush(hand2)) return true;
        if (HasFlush(hand2) && !HasFlush(hand1)) return false;

        int straight1 = HasStraight(hand1);
        int straight2 = HasStraight(hand2);
        if (straight1 + straight2 > 0) return straight1 > straight2;

        int three1 = HasThree(hand1);
        int three2 = HasThree(hand2);
        if (three1 + three2 != 0)
            return three1 > three2;

        (a, b) = HasTwoPair(hand1);
        (c, d) = HasTwoPair(hand2);

        if (a + b + c + d != 0) {
            if (a != 0 && c == 0) return true;
            if (c != 0 && a == 0) return false;
        }

        int pair1 = HasPair(hand1);
        int pair2 = HasPair(hand2);
        if (pair1 + pair2 != 0) return pair1 > pair2;

        return HighCard(hand1) > HighCard(hand2);
    }
    
    private List<Card> RemoveCard(int value, List<Card> hand) {
        var copy = new List<Card>(hand);
        int index = copy.FindIndex(c => c.Value == value);
        if (index != -1) copy.RemoveAt(index);
        return copy;
    }
    
    private bool HasFlush(List<Card> hand) => hand.All(c => c.Suit == hand[0].Suit);
    
    private int HasStraight(List<Card> hand) {
        var values = hand.Select(c => c.Value).OrderByDescending(v => v).ToList();
        for (int i = 0; i < values.Count - 1; i++)
            if (values[i] - 1 != values[i + 1]) return 0;
        return values[0];
    }
    
    private (int, int) HasFullHouse(List<Card> hand) {
        int three = HasThree(hand);
        if (three == 0) return (0, 0);
        var temp = RemoveCard(three, RemoveCard(three, RemoveCard(three, hand)));
        (int a, int b) = HasTwoPair(temp);
        if (a == 0) return (0, 0);
        if (a == three) return (three, b);
        return b == three ? (three, a) : (0, 0);
    }
    
    private int HasThree(List<Card> hand) {
        for (int i = 0; i < hand.Count; i++)
        for (int j = i + 1; j < hand.Count; j++)
        for (int k = j + 1; k < hand.Count; k++)
            if (hand[i].Value == hand[j].Value && hand[j].Value == hand[k].Value)
                return hand[i].Value;
        return 0;
    }
    
    private (int, int) HasTwoPair(List<Card> hand) {
        int first = HasPair(hand);
        if (first == 0) return (0, 0);
        var reduced = RemoveCard(first, RemoveCard(first, hand));
        int second = HasPair(reduced);
        return second == 0 ? (0, 0) : (first, second);
    }
    
    private int HasPair(List<Card> hand) {
        for (int i = 0; i < hand.Count; i++) 
        for (int j = i + 1; j < hand.Count; j++) 
            if (hand[i].Value == hand[j].Value) 
                return hand[i].Value;
        return 0;
    }
    
    private int HighCard(List<Card> hand) => hand.Max(c => c.Value);

    private class Card {
        public int Value { get; }
        public string Suit { get; }

        public Card(string s) {
            string rank = s.Substring(0, 1);
            Suit = s.Substring(1, 1);
            Value = rank switch {
                "T" => 10,
                "J" => 11,
                "Q" => 12,
                "K" => 13,
                "A" => 14,
                _ => int.Parse(rank)
            };
        }
    }
}

