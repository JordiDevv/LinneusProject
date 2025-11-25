using System.Collections.Generic;
using Domain.CardsSystem.Definitions;
using System.Linq;

namespace Domain.State
{
    public class PlayerState
    {
        public Deck Deck { get; }
        public List<Card> Hand { get; } = new();
        public List<Card> Field { get; } = new();
        public int Points { get; private set; } = 0;

        public PlayerState(Deck deck) => Deck = deck;

        public void UpdateHandForPhase(Phase currentPhase)
        {
            Hand.Clear();
            foreach (var card in Deck.Cards)
            {
                if (card.CanBePlayedInPhase(currentPhase))
                    Hand.Add(card);
            }
        }

        public void PlayCard(Card card)
        {
            if (Hand.Contains(card) && Deck.RemoveCard(card))
            {
                Hand.Remove(card);
                Field.Add(card);
                card.Play();
            }
        }

        public void CountPoints()
        {
            for (int i = 0; i < this.Field.Count; i++)
            {
                if (this.Field[i] is AnimalCard animalCard)
                    Points += animalCard.Stats.Values.Sum();
            }
        }

        public void ResetField() => Field.Clear();
    }
}
