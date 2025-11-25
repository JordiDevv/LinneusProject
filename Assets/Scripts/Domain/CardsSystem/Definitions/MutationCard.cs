using Domain.Stats;
using Domain.State;

namespace Domain.CardsSystem.Definitions
{
    public class MutationCard : Card
    {
        public string TaxonName { get; private set; }
        public StatsType Stat { get; private set; }
        public int Value { get; private set; }
        public bool AllStats { get; private set; }
        public bool Team { get; private set; }

        public MutationCard(string name, string taxonName, StatsType stat, int value, bool allStats, bool team)
        : base(name)
        {
            TaxonName = taxonName;
            Stat = stat;
            Value = value;
            AllStats = allStats;
            Team = team;
        }

        public override bool CanBePlayedInPhase(Phase phase)
        {
            if (phase == Phase.MutationPhase) return !this.Team;
            else if (phase == Phase.TeamPhase) return this.Team;
            else return false;
        }

        public override Card Clone() => new MutationCard(Name, TaxonName, Stat, Value, AllStats, Team);
    }
}
