using UnityEngine;
using UnityEngine.UI;
using Domain.CardsSystem.Definitions;
using Domain.State;
using System.Collections;
using System.Collections.Generic;
using Data;

public class SpawnCards : MonoBehaviour
{
    public GameObject animalCardPrefab;
    public GameObject mutationCardPrefab;
    public MatchController matchController;

    public void SpawnPlayerHand(PlayerState player, List<IList> allCardsData, Transform container)
    {
        foreach (var card in player.Hand)
        {
            bool found = false;

            foreach (IList list in allCardsData)
            {
                foreach (var item in list)
                {
                    if (item is ICardData data && data.CardName == card.Name)
                    {
                        SpawnCardVisual(card, data.Image, container);
                        found = true;
                        break;
                    }
                }
                if (found) break;
            }
        }
    }

    public void SpawnCardVisual(Card card, Sprite image, Transform container)
    {
        GameObject cardGO = null;
        switch (card.GetType().Name)
        {
            case "AnimalCard":
                cardGO = Instantiate(animalCardPrefab, container);
                break;
            case "MutationCard":
                cardGO = Instantiate(mutationCardPrefab, container);
                break;
            default:
                break;
        }

        CardDisplay display = null;
        if (cardGO)
            display = cardGO.GetComponent<CardDisplay>();
        if (display)
        {
            display.SetMatchController(matchController);
            display.Setup(card, image);
        }
    }

    public void UpdateVisualHand(PlayerState player, List<IList> allCardsData, Transform container)
    {
        for (int i = 0; i < container.childCount; i++)
            Destroy(container.GetChild(i).gameObject);

        SpawnPlayerHand(player, allCardsData, container);
    }

    public void ClearVisualField(Transform container)
    {
        for (int i = 0; i < container.childCount; i++)
            Destroy(container.GetChild(i).gameObject);
    }
}
