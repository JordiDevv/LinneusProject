using Domain.Event;
using Domain.State;

namespace Domain.CardsSystem.Definitions
{
    public class EventCard : Card
    {
        public Habitat Habitat { get; }
        public Discipline Discipline { get; }

        public EventCard(string name, Habitat habitat, Discipline discipline)
        : base(name)
        {
            Habitat = new Habitat(habitat.Name);
            Discipline = new Discipline(discipline.Name, discipline.Modality, discipline.Year, discipline.Multipliers);
        }

        public override bool CanBePlayedInPhase(Phase phase) => phase == Phase.EventPhase;

        public override Card Clone() => new EventCard(Name, Habitat, Discipline);
    }
}
