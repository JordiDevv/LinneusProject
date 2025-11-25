using System.Collections.Generic;
using System;

namespace Domain.CardsSystem.Definitions
{
    public class Deck
    {
        private readonly List<Card> _cards;
        public IReadOnlyList<Card> Cards => _cards;

        public Deck(IEnumerable<Card> startingCards) => _cards = new List<Card>(startingCards);

        public void Add(Card card) => _cards.Add(card);

        public bool RemoveCard(Card card) => _cards.Remove(card);
    }
}
