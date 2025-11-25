using System.Linq;
using Domain.CardsSystem.Definitions;
using Domain.State;
using Domain.Stats;

namespace Domain.CardsSystem.Effects
{
    public static class MutationEffects
    {
        public static void ApplyMutation(PlayerState player, AnimalCard animalCard, MutationCard mutationCard)
        {
            if (mutationCard.Team)
                ApplyTeam(player, mutationCard);
            else
                ApplyIndividual(animalCard, mutationCard);
        }

        public static void ApplyTeam(PlayerState player, MutationCard mutationCard)
        {
            for (int i = 0; i < player.Field.Count; i++)
            {
                if (player.Field[i] is AnimalCard animalCard)
                    ApplyIndividual(animalCard, mutationCard);
            }
        }

        public static void ApplyIndividual(AnimalCard animalCard, MutationCard mutationCard)
        {
            if (mutationCard.AllStats)
            {
                foreach (var stat in animalCard.Stats.ToList())
                    animalCard.ModifyStat(stat.Key, mutationCard.Value, StatOperation.Add);
            }
            else
                animalCard.ModifyStat(mutationCard.Stat, mutationCard.Value, StatOperation.Add);
        }
    }
}
