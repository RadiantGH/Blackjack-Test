using System;
using System.Collections.Generic;
using System.Text;
using Blackjack_Test.Classes;
using System.Linq;

namespace Blackjack_Test
{
    public static class Blackjack
    {
        //BLACKJACK RULES
        const int MAX_VALUE = 21;
        const int ACE_MIN = 1;
        const int ACE_MAX = 11;

        static int gamesWon;
        static int gamesLost;

        static Card[] cardlist;
        static List<Card> deck;
        static List<Card> dealerHand;
        static List<Card> playerHand;

        public static void Startup()
        {
            Console.WriteLine("Welcome to Blackjack. Press any key to get started!");

            Console.ReadKey();

            Console.WriteLine();

            GenerateCards();
            StartGame();
        }

        static void GenerateCards()
        {
            List<Card> unbuiltList = new List<Card>();

            for(int i = 0; i < Enum.GetValues(typeof(Suit)).Length; i++)
            {
                for(int b = 1; b <= Enum.GetValues(typeof(Value)).Length; b++)
                {
                    Card c = new Card((Suit)i, (Value)b);
                    unbuiltList.Add(c);
                }
            }

            cardlist = unbuiltList.ToArray();
        }

        static void StartGame()
        {
            Console.Clear();

            ResetGame();

            PlayerDraw();
            PlayerDraw();
            DealerDraw();
            DealerDraw();

            int gameResult = CheckVictory(opponentTurn: false);
            if(gameResult == 0 || gameResult == 1 || gameResult == 2)
            {
                EndGame(gameResult);
            }
            else
            {
                PromptPlayer();
            }
        }

        static void EndGame(int gameResult, bool surrender = false)
        {
            bool dealerBJ = false;
            int dealerValue = CalculateHandValue(dealerHand, out dealerBJ);
            string dealerString = GetHandString(dealerHand);

            bool playerBJ = false;
            int playerValue = CalculateHandValue(playerHand, out playerBJ);
            string playerString = GetHandString(playerHand);

            Console.WriteLine();

            if(surrender)
            {
                Console.WriteLine("YOU SURRENDERED!");
            }
            else
            {
                Console.WriteLine("Your hand: " + playerString);

                if(playerBJ)
                {
                    Console.WriteLine("Your value: BLACKJACK!");
                }
                else
                {
                    Console.WriteLine("Your value: " + playerValue);
                }

                Console.WriteLine("VS");

                Console.WriteLine("Dealer's hand: " + dealerString);

                if (dealerBJ)
                {
                    Console.WriteLine("Dealer's value: BLACKJACK!");
                }
                else
                {
                    Console.WriteLine("Dealer's value: " + dealerValue);
                }

                if (gameResult == 0)
                {
                    Console.WriteLine("DRAW!");
                }
                else if (gameResult == 1)
                {
                    Console.WriteLine("DEFEAT!");
                }
                else
                {
                    Console.WriteLine("VICTORY!");
                }
            }

            Console.WriteLine("Play again (Z) or Quit (X) ?");
            if(TakeYesNo())
            {
                StartGame();
            }
            else
            {
                Console.WriteLine("Thanks for playing!\nPress any key to end the program...");
                Console.ReadKey();
                Environment.Exit(-1);
            }
        }

        static void ResetGame()
        {
            deck = cardlist.ToList();
            ShuffleCards();

            dealerHand = new List<Card>();
            playerHand = new List<Card>();
        }

        #region Actions
        static void PromptPlayer()
        {
            ReadHandInfo();

            Console.WriteLine();
            Console.WriteLine("Hit (Z), Stand (X), or Surrender(C) ?");
            var key = Console.ReadKey();

            if(key.Key == ConsoleKey.Z)
            {
                Hit();
            }
            else if(key.Key == ConsoleKey.X)
            {
                Stand();
            }
            else if(key.Key == ConsoleKey.C)
            {
                Surrender();
            }
        }

        static void Hit()
        {
            Console.WriteLine();
            Console.WriteLine("You draw a card...");
            Card c = PlayerDraw();
            Console.WriteLine("...");

            int gameResult = CheckVictory(opponentTurn: false);
            if (gameResult == 0 || gameResult == 1 || gameResult == 2)
            {
                EndGame(gameResult);
                return;
            }
            else
            {
                PromptPlayer();
            }
        }

        static void Stand()
        {
            Console.WriteLine();
            int count = 0;
            int gameResult = 0;

            while (true)
            {
                gameResult = CheckVictory(true);

                if (gameResult == 0 || gameResult == 1 || gameResult == 2)
                {
                    break;
                }
                else
                {
                    count++;
                    DealerDraw();
                }
            }

            Console.WriteLine("The dealer draws " + count + " times and...");
            EndGame(gameResult);
        }

        static void Surrender()
        {
            Console.WriteLine();
            EndGame(1, true);
        }
        #endregion

        #region Game Mechanic
        //0 = draw
        //1 = defeat
        //2 = victory
        //3 = draw more, dealer!
        //4 = nothing!
        static int CheckVictory(bool opponentTurn)
        {
            bool playerBlackjack = false;
            int playerValue = CalculateHandValue(playerHand, out playerBlackjack);

            bool dealerBlackjack = false;
            int dealerValue = CalculateHandValue(dealerHand, out dealerBlackjack);

            //Blackjack
            if(playerBlackjack)
            {
                if(dealerBlackjack)
                {
                    return 0;
                }
                else
                {
                    return 2;
                }
            }
            if(dealerBlackjack)
            {
                return 1;
            }

            //Over max
            if(playerValue > MAX_VALUE)
            {
                if(dealerValue > MAX_VALUE)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            if(dealerValue > MAX_VALUE)
            {
                return 2;
            }

            //Is max
            if(playerValue == MAX_VALUE)
            {
                if(dealerValue == MAX_VALUE)
                {
                    return 0;
                }
                else
                {
                    return 2;
                }
            }

            //First check doesn't compare the two values. Only checks for a sure-win
            if (!opponentTurn) 
            {
                return 4;
            }
            else
            {
                if(playerValue >= dealerValue)
                {
                    return 3;
                }
                else
                {
                    return 1;
                }
            }
        }

        static Card PlayerDraw()
        {
            int randomIndex = new Random().Next(0, deck.Count);

            Card c = deck[randomIndex];
            playerHand.Add(c);
            deck.RemoveAt(randomIndex);

            return c;
        }

        static Card DealerDraw()
        {
            int randomIndex = new Random().Next(0, deck.Count);

            Card c = deck[randomIndex];
            dealerHand.Add(c);
            deck.RemoveAt(randomIndex);

            return c;
        }
        #endregion

        #region Utility
        static void ReadHandInfo()
        {
            Console.WriteLine("Your Hand: ");
            string s = GetHandString(playerHand);

            bool trash;
            Console.WriteLine(s);
            Console.WriteLine("Value: " + CalculateHandValue(playerHand, out trash));
        }

        static string GetHandString(List<Card> hand)
        {
            string s = "";
            for (int i = 0; i < hand.Count; i++)
            {
                Card c = hand[i];
                string suitName = Enum.GetName(typeof(Suit), c.suit);
                string valName = Enum.GetName(typeof(Value), c.val);

                s += "[" + valName + " of " + suitName + "]";

                if (i < hand.Count - 1)
                {
                    s += ", ";
                }
            }

            return s;
        }

        static int CalculateHandValue(List<Card> cards, out bool blackjack)
        {
            int val = 0;

            int aceCount = 0;
            blackjack = false;

            for(int i = 0; i < cards.Count; i++)
            {
                Card c = cards[i];

                if (c.val == Value.Jack && c.IsBlack)
                {
                    blackjack = true;
                    return 0;
                }

                if (c.val == Value.Ace)
                {
                    val += ACE_MAX;
                }
                else
                {
                    val += Math.Min((int)c.val, 10);
                }
            }

            for(int i  = 0; i < aceCount; i++)
            {
                if(val > MAX_VALUE)
                {
                    val -= (ACE_MAX - ACE_MIN);
                }
            }

            return val;
        }

        static void ShuffleCards()
        {
            deck = deck.OrderBy(a => Guid.NewGuid()).ToList();
        }

        static bool TakeYesNo()
        {
           var c = Console.ReadKey();
           
            if(c.Key == ConsoleKey.Z)
            {
                return true;
            }

            return false;
        }
        #endregion

    }
}
