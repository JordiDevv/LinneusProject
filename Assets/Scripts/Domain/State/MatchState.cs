using Domain.CardsSystem.Definitions;

namespace Domain.State
{
    public enum Phase
    {
        EventPhase,
        AnimalPhase,
        MutationPhase,
        TeamPhase
    }

    public class MatchState
    {
        public PlayerState Player1 { get; set; }
        public PlayerState Player2 { get; set; }
        public EventState Event { get; } = new();
        public Phase Phase { get; set; } = Phase.AnimalPhase;
        public bool SelectingAnimalTarget { get; set; } = false;
        public MutationCard MutationToApply { get; set; } = null;
        public int MutationsPlayed { get; private set; } = 0;
        public int Round { get; private set; } = 0;

        public void PlayEventCard(EventCard card) => Event.Play(card);

        public void IncreaseMutation() => MutationsPlayed++;

        public void MutationState(MutationCard mutationCard, bool activate)
        {
            SelectingAnimalTarget = activate;
            MutationToApply = mutationCard;
        }

        public void ChangePhase(Phase newPhase)
        {
            Phase = newPhase;
            Player1.UpdateHandForPhase(Phase);
            Player2.UpdateHandForPhase(Phase);
        }

        public void AdaptStatsToEvent()
        {
            for (int i = 0; i < Player1.Field.Count; i++)
            {
                if (Player1.Field[i] is AnimalCard animalCard)
                    Event.ApplyMultipliers(animalCard);
            }
            for (int i = 0; i < Player2.Field.Count; i++)
            {
                if (Player2.Field[i] is AnimalCard animalCard)
                    Event.ApplyMultipliers(animalCard);
            }
        }

        public void NextRound()
        {
            Round++;
            Phase = Phase.AnimalPhase;

            Player1.ResetField();
            Player2.ResetField();
            Event.Reset();
            MutationsPlayed = 0;

            Player1.UpdateHandForPhase(Phase);
            Player2.UpdateHandForPhase(Phase);
        }
    }
}
