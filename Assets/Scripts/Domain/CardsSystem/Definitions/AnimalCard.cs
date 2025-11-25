using System.Collections.Generic;
using System;
using Domain.Stats;
using Domain.Rarities;
using Domain.State;
using Domain.CardsSystem.Effects;

namespace Domain.CardsSystem.Definitions
{
    public class AnimalCard : Card
    {
        public string ScientificName { get; private set; }
        public Rarity Rarity { get; private set; }
        private Dictionary<StatsType, int> _stats;
        public IReadOnlyDictionary<StatsType, int> Stats => _stats;
        public bool IsMutated;

        public AnimalCard(string name, string scientificName, RarityType rarity, Dictionary<StatsType, int> stats)
        : base(name)
        {
            ScientificName = scientificName;
            Rarity = new Rarity(rarity);
            this._stats = new Dictionary<StatsType, int>(stats);
            this.IsMutated = false;
        }

        public override bool CanBePlayedInPhase(Phase phase) => phase == Phase.AnimalPhase;

        public void ModifyStat(StatsType stat, int value, StatOperation op)
        {
            switch (op)
            {
                case StatOperation.Add:
                    _stats[stat] += value;
                    break;
                case StatOperation.Subtract:
                    _stats[stat] -= value;
                    break;
                case StatOperation.Set:
                    _stats[stat] = value;
                    break;
                default:
                    Console.WriteLine($"Warning: operator not valid '{op}'.");
                    break;
            }
        }

        public void Mutate(MutationCard mutationToApply)
        {
            IsMutated = true;
            MutationEffects.ApplyIndividual(this, mutationToApply);
        }

        public override Card Clone() => new AnimalCard(Name, ScientificName, Rarity.Type, _stats);
    }
}
