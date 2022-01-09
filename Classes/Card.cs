using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack_Test.Classes
{
    public class Card
    {
        public Suit suit;
        public Value val;
        public bool IsBlack { get { return suit == Suit.Clubs || suit == Suit.Spades; } }

        public Card(Suit s, Value v)
        {
            suit = s;
            val = v;
        }
    }

    //Enums
    public enum Suit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }

    public enum Value
    {
        Ace = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13
    }
}
