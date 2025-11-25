using Domain.Stats;

namespace Domain.Event
{
    public class Discipline
    {
        public string Name { get; }
        public string Modality { get; }
        public string Year { get; }
        public StatsMultipliers Multipliers { get; }

        public Discipline(string name, string modality, string year, StatsMultipliers multipliers)
        {
            Name = name;
            Modality = modality;
            Year = year;
            Multipliers = multipliers;
        }
    }
}
