using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;
using Domain.CardsSystem.Definitions;
using Domain.Event;
using Domain.State;
using Domain.Stats;
using Data;

public static class Initializer
{
    private class CardListPair<TData, TRef>
    {
        public List<TData> Data { get; set; }
        public List<TRef> Ref { get; set; }
    }
    public static void FilterData(List<IList> allCardsData, List<object> allCardsRef)
    {
        var pairs = CreateCardPairs(allCardsData, allCardsRef);

        foreach (var pair in pairs)
        {
            switch (pair)
            {
                case CardListPair<AnimalCardData, AnimalCard> animalPair:
                    FilterAnimal(animalPair.Data, animalPair.Ref);
                    break;
                case CardListPair<MutationCardData, MutationCard> mutationPair:
                    FilterMutation(mutationPair.Data, mutationPair.Ref);
                    break;
                case CardListPair<EventCardData, EventCard> eventPair:
                    FilterEvent(eventPair.Data, eventPair.Ref);
                    break;
                default:
                    Console.WriteLine("Error filtering the cards data");
                    break;
            }
        }
    }

    public static List<object> CreateCardPairs(List<IList> allData, List<object> allRef)
    {
        var result = new List<object>();

        foreach (var dataList in allData)
        {
            Type dataType = dataList.GetType().GetGenericArguments()[0];
            string refTypeName = dataType.Name.Replace("Data", "");

            Type refType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name == refTypeName);

            if (refType == null)
            {
                Console.WriteLine($"Ref {dataType.Name} wasn't found");
                continue;
            }

            var matchingRefList = allRef.FirstOrDefault(r => r.GetType().GetGenericArguments()[0] == refType);

            if (matchingRefList == null)
            {
                Console.WriteLine($"List for the ref {refTypeName} wasn't found");
                continue;
            }

            var pairType = typeof(CardListPair<,>).MakeGenericType(dataType, refType);
            var pairInstance = Activator.CreateInstance(pairType);

            pairType.GetProperty("Data").SetValue(pairInstance, dataList);
            pairType.GetProperty("Ref").SetValue(pairInstance, matchingRefList);

            result.Add(pairInstance);
        }

        return result;
    }


    public static void FilterAnimal(List<AnimalCardData> data, List<AnimalCard> list)
    {
        foreach (var dataItem in data)
        {
            var statsDict = dataItem.Stats.ToDictionary(stat => stat.StatType, stat => stat.Value);

            AnimalCard newCard = new AnimalCard(
                dataItem.CardName,
                dataItem.ScientificName,
                dataItem.Rarity,
                statsDict
            );

            list.Add(newCard);
        }
    }

    public static void FilterMutation(List<MutationCardData> data, List<MutationCard> list)
    {
        foreach (var dataItem in data)
        {
            MutationCard newCard = new MutationCard(
                dataItem.CardName,
                dataItem.TaxonName,
                dataItem.Stat,
                dataItem.Value,
                dataItem.AllStats,
                dataItem.Team
            );

            list.Add(newCard);
        }
    }
    
    public static void FilterEvent(List<EventCardData> data, List<EventCard> list)
    {
        foreach (var dataItem in data)
        {
            Habitat habitat = new Habitat(dataItem.Habitat);

            var multiDict = dataItem.Multipliers.ToDictionary(multi => multi.Stat, multi => multi.Multiplier);
            StatsMultipliers multipliers = new StatsMultipliers(multiDict);
            Discipline discipline = new Discipline(dataItem.Discipline, dataItem.Modality, dataItem.Year, multipliers);

            EventCard newCard = new EventCard(
                dataItem.CardName,
                habitat,
                discipline
            );

            list.Add(newCard);
        }
    }
}
