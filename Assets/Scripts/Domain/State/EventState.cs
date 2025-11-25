using System.Collections.Generic;
using Domain.CardsSystem.Definitions;
using Domain.Stats;

namespace Domain.State
{
    public class EventState
    {
        public EventCard CurrentEventCard { get; private set; }
        public bool IsActive => CurrentEventCard?.IsPlayed == true;

        public void Play(EventCard card)
        {
            if (!IsActive)
            {
                CurrentEventCard = card;
                card.Play();
            }
        }

        public void ApplyMultipliers(AnimalCard card)
        {
            if (!IsActive) return;

            var multipliers = CurrentEventCard.Discipline.Multipliers;
            var statKeys = new List<StatsType>(card.Stats.Keys);
            foreach (var stat in statKeys)
            {
                float multiplier = multipliers.GetMultiplierFor(stat);
                int baseStat = card.Stats[stat];
                int result = (int)(baseStat * multiplier);
                card.ModifyStat(stat, result, StatOperation.Set);
            }
        }

        public void Reset() => CurrentEventCard = null;
    }
}
