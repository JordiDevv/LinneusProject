using System.Collections.Generic;
using System.Linq;
using Domain.CardsSystem.Definitions;
using Domain.Stats;
using UnityEngine;

public static class AnimalCardFactory
{
    public static AnimalCard CreateFromData(AnimalCardData data)
    {
        var statsDict = data.Stats.ToDictionary(entry => entry.StatType, entry => entry.Value);
        
        return new AnimalCard(
            data.CardName,
            data.ScientificName,
            data.Rarity,
            statsDict
        );
    }
}
