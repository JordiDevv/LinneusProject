using System.Collections.Generic;
using Domain.CardsSystem.Definitions;
using Domain.Stats;
using Domain.Event;

public static class EventCardFactory
{
    public static EventCard CreateFromData(EventCardData data)
    {
        var dict = new Dictionary<StatsType, float>();
        foreach (var entry in data.Multipliers)
        {
            dict[entry.Stat] = entry.Multiplier;
        }

        var multipliers = new StatsMultipliers(dict);
        var name = data.CardName;
        var discipline = new Discipline(data.Discipline, data.Modality, data.Year, multipliers);
        var habitat = new Habitat(data.Habitat);

        return new EventCard(name, habitat, discipline);
    }
}
