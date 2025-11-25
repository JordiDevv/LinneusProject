using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Domain.CardsSystem.Definitions;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI extraNameText;
    [SerializeField] private Image cardImage;

    public Card Card { get; private set; }
    private MatchController matchController;

    public void Setup(Card card, Sprite image)
    {
        Card = card;

        nameText.text = card.Name;
        cardImage.sprite = image;

        if (card is AnimalCard animalCard)
            extraNameText.text = animalCard.ScientificName;
        else if (card is MutationCard mutationCard)
            extraNameText.text = mutationCard.TaxonName;
    }

    public void SetMatchController(MatchController controller)
    {
        matchController = controller;
    }

    public void OnClick()
    {
        matchController.OnCardClicked(this);
    }
}
