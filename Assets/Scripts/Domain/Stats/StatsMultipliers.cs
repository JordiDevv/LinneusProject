using System.Collections.Generic;
using System;

namespace Domain.Stats
{
    public class StatsMultipliers
    {
        private readonly Dictionary<StatsType, float> _multipliers;

        public StatsMultipliers(Dictionary<StatsType, float> multipliers)
        {
            _multipliers = multipliers ?? new Dictionary<StatsType, float>();
        }

        public float GetMultiplierFor(StatsType stat)
        {
            return _multipliers.TryGetValue(stat, out var multiplier) ? multiplier : 0f;
        }

        public IReadOnlyDictionary<StatsType, float> Multipliers => _multipliers;
    }
}
