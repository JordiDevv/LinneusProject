using Domain.Rarities;
using Domain.CardsSystem.Definitions;
using Domain.Stats;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Domain.CardsSystem.Services
{
    public static class DeckBuilder
    {
        public static readonly Random random = new Random();

        public static void FillDeck(Deck deck, List<object> allCardsRef)
        {
            foreach (var list in allCardsRef)
            {
                switch (list)
                {
                    case List<AnimalCard> animalCards:
                        DealAnimals(deck, animalCards);
                        break;
                    case List<MutationCard> mutationCards:
                        DealMutations(deck, mutationCards);
                        break;
                    case List<EventCard> eventCards:
                        DealEvents(deck, eventCards);
                        break;
                    default:
                        Console.WriteLine("Error filling the decks");
                        break;
                }
            }
        }

        public static void DealAnimals(Deck deck, List<AnimalCard> animalCards)
        {
            for (int i = 0; i < 10; i++)
            {
                AnimalCard card = GetRandomCardByRarity(animalCards);
                deck.Add(card.Clone());
            }
        }

        public static void DealMutations(Deck deck, List<MutationCard> mutationCards)
        {
            List<MutationCard> singleMutations = new List<MutationCard>();
            List<MutationCard> teamMutations = new List<MutationCard>();

            foreach (var card in mutationCards)
            {
                if (card.Team)
                    teamMutations.Add(card);
                else
                    singleMutations.Add(card);
            }

            for (int i = 0; i < 5; i++)
            {
                var randomIndex = random.Next(0, singleMutations.Count);
                MutationCard card = singleMutations[randomIndex];
                deck.Add(card.Clone());
            }
            for (int i = 0; i < 3; i++)
            {
                var randomIndex = random.Next(0, teamMutations.Count);
                MutationCard card = teamMutations[randomIndex];
                deck.Add(card.Clone());
            }
        }

        public static void DealEvents(Deck deck, List<EventCard> eventCards)
        {
            for (int i = 0; i < 2; i++)
            {
                var randomIndex = random.Next(0, eventCards.Count);
                EventCard card = eventCards[randomIndex];
                deck.Add(card.Clone());
            }
        }

        public static AnimalCard GetRandomCardByRarity(List<AnimalCard> animalCards)
        {
            int rarityPercent = random.Next(1, 101);
            RarityType rarityTarget = rarityPercent switch
            {
                >= 0 and <= 50 => RarityType.Daily,
                >= 51 and <= 80 => RarityType.Peculiar,
                >= 81 and <= 95 => RarityType.Singular,
                >= 96 and <= 99 => RarityType.Majestic,
                100 => RarityType.Epic,
                _ => RarityType.Daily
            };

            List<AnimalCard> cardsFiltred = new List<AnimalCard>();
            foreach (var card in animalCards)
            {
                if (card.Rarity.Type == rarityTarget)
                    cardsFiltred.Add(card);
            }

            var randomIndex = random.Next(0, cardsFiltred.Count);
            var randomCard = cardsFiltred[randomIndex];

            return randomCard;
        }
    }
}
