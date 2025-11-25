using Domain.State;
using System;

namespace Domain.CardsSystem.Definitions
{
    public abstract class Card
    {
        public Guid InstanceId { get; private set; }
        public string Name { get; private set; }
        public bool IsPlayed { get; private set; }

        protected Card(string name)
        {
            InstanceId = Guid.NewGuid();
            Name = name;
            IsPlayed = false;
        }

        public abstract bool CanBePlayedInPhase(Phase phase);

        public virtual void Play() => IsPlayed = true;

        public abstract Card Clone();
    }
}
