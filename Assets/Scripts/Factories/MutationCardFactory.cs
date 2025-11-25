using UnityEngine;
using Domain.CardsSystem.Definitions;
using Domain.Stats;

public class MutationCardFactory
{
    public static MutationCard CreateFromData(MutationCardData data)
    {
        return new MutationCard(
            data.CardName,
            data.TaxonName,
            data.Stat,
            data.Value,
            data.AllStats,
            data.Team
        );
    }
}
