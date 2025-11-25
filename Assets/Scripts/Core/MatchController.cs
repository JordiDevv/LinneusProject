using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Domain.CardsSystem.Definitions;
using Domain.CardsSystem.Services;
using Domain.Stats;
using Domain.State;
using Domain.Rarities;
using TMPro;
using Data;
using Domain.CardsSystem.Effects;
using UnityEngine.Analytics;

public class MatchController : MonoBehaviour
{
    [SerializeField] private List<AnimalCardData> allAnimalCards;
    [SerializeField] private List<MutationCardData> allMutationCards;
    [SerializeField] private List<EventCardData> allAvailableEvents;
    [SerializeField] private List<GameObject> multipliersGO;
    [SerializeField] private List<Sprite> multipliersImages;
    [SerializeField] private GameObject animalCardPrefab;
    [SerializeField] private GameObject disciplineLogo;
    [SerializeField] private GameObject background;
    [SerializeField] private TextMeshProUGUI yearText;
    [SerializeField] private TextMeshProUGUI disciplineText;
    [SerializeField] private GameObject multipliersControllerGO;
    [SerializeField] private Transform handContainer;
    [SerializeField] private Transform playerFieldContainer;
    [SerializeField] private Transform enemyFieldContainer;
    [SerializeField] private GameObject resultContainer;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private GameObject blocker;

    private SpawnCards spawnCards;
    private MatchState matchState = new MatchState();
    private MultipliersDisplay multipliersDisplay;
    private PlayerState player;
    private List<Card> playerDefaultDeck = new List<Card>();
    private Deck playerDeck;
    private PlayerState enemy;
    private List<Card> enemyDefaultDeck = new List<Card>();
    private Deck enemyDeck;

    private List<IList> allCardsData;
    private List<AnimalCard> animalCardsRef = new List<AnimalCard>();
    private List<MutationCard> mutationCardsRef = new List<MutationCard>();
    private List<EventCard> eventCardsRef = new List<EventCard>();
    private List<object> allCardsRef;

    void Awake()
    {
        spawnCards = GetComponent<SpawnCards>();
    }

    void Start()
    {
        playerDeck = new Deck(playerDefaultDeck);
        enemyDeck = new Deck(enemyDefaultDeck);
        allCardsData = new List<IList> { allAnimalCards, allMutationCards, allAvailableEvents };
        allCardsRef = new List<object> { animalCardsRef, mutationCardsRef, eventCardsRef };

        Initializer.FilterData(allCardsData, allCardsRef);
        DeckBuilder.FillDeck(playerDeck, allCardsRef);
        DeckBuilder.FillDeck(enemyDeck, allCardsRef);

        player = new PlayerState(playerDeck);
        enemy = new PlayerState(enemyDeck);
        matchState.Player1 = player;
        matchState.Player2 = enemy;

        StartRound();
    }

    void StartRound()
    {
        matchState.NextRound();
        
        var eventCardData = allAvailableEvents[Random.Range(0, allAvailableEvents.Count)];
        allAvailableEvents.Remove(eventCardData);
        var eventCard = EventCardFactory.CreateFromData(eventCardData);

        matchState.PlayEventCard(eventCard);
        multipliersDisplay = multipliersControllerGO.GetComponent<MultipliersDisplay>();

        GetRoundInfo(eventCard, eventCardData);
        if (matchState.Round == 1)
            spawnCards.SpawnPlayerHand(player, allCardsData, handContainer);
        else if (matchState.Round == 2)
        {
            spawnCards.ClearVisualField(playerFieldContainer);
            spawnCards.ClearVisualField(enemyFieldContainer);
            spawnCards.UpdateVisualHand(player, allCardsData, handContainer);
        }
    }

    void GetRoundInfo(EventCard eventCard, EventCardData eventCardData)
    {
        disciplineLogo.GetComponent<Image>().sprite = eventCardData.Logo;
        background.GetComponent<Image>().sprite = eventCardData.Image;
        yearText.text = eventCard.Discipline.Year;
        disciplineText.text = eventCard.Discipline.Name + " - " + eventCard.Discipline.Modality;
        multipliersDisplay.SetMultipliers(eventCard, multipliersGO, multipliersImages);
    }

    public void OnCardClicked(CardDisplay display)
    {
        Card card = display.Card;

        if (card is AnimalCard animalCard)
        {
            if (matchState.Phase == Phase.AnimalPhase)
            {
                if (!player.Hand.Contains(animalCard))
                    return;
                player.PlayCard(animalCard);
                display.transform.SetParent(playerFieldContainer);
            }
            else if (!matchState.SelectingAnimalTarget)
                return;
            else if (matchState.SelectingAnimalTarget)
            {
                if (!player.Field.Contains(animalCard) || animalCard.IsMutated)
                    return;
                player.PlayCard(matchState.MutationToApply);
                animalCard.Mutate(matchState.MutationToApply);
                matchState.MutationState(null, false);
            }
        }
        else if (card is MutationCard mutationCard)
        {
            if (mutationCard.Team)
            {
                player.PlayCard(mutationCard);
                MutationEffects.ApplyTeam(player, mutationCard);
            }
            else
                matchState.MutationState(mutationCard, true);
        }

        if (matchState.Phase == Phase.MutationPhase || matchState.Phase == Phase.TeamPhase)
            spawnCards.UpdateVisualHand(player, allCardsData, handContainer);
        if (!matchState.SelectingAnimalTarget)
        {
            blocker.SetActive(true);
            Invoke(nameof(EnemyPlaysCard), 1f);
        }
    }

    void EnemyPlaysCard()
    {
        if (enemy.Hand.Count == 0) return;

        var card = enemy.Hand[Random.Range(0, enemy.Hand.Count)];
        if (card is AnimalCard animalCard && matchState.Phase == Phase.AnimalPhase)
        {
            enemy.PlayCard(animalCard);
            var cardData = allAnimalCards.Find(data => data.CardName == card.Name);
            if (cardData)
                spawnCards.SpawnCardVisual(card, cardData.Image, enemyFieldContainer);
        }
        else if (card is MutationCard mutationCard)
        {
            if (mutationCard.Team)
                MutationEffects.ApplyTeam(enemy, mutationCard);
            else
            {
                var nonMutatedAnimalCards = enemy.Field.OfType<AnimalCard>().Where(c => !c.IsMutated).ToList();

                Card targetCard = nonMutatedAnimalCards[Random.Range(0, nonMutatedAnimalCards.Count)];
                if (targetCard is AnimalCard targetAnimalCard)
                {
                    targetAnimalCard.Mutate(mutationCard);
                    enemy.PlayCard(mutationCard);
                }
            }
        }

        PhaseControl();
        blocker.SetActive(false);
    }

    void PhaseControl()
    {
        if (matchState.Phase == Phase.AnimalPhase && enemy.Field.Count == 4)
        {
            matchState.ChangePhase(Phase.MutationPhase);
            spawnCards.UpdateVisualHand(player, allCardsData, handContainer);
        }
        else if (matchState.Phase == Phase.MutationPhase && matchState.MutationsPlayed < 2)
        {
            matchState.IncreaseMutation();
        }
        if (matchState.Phase == Phase.MutationPhase && matchState.MutationsPlayed >= 2)
        {
            matchState.ChangePhase(Phase.TeamPhase);
            spawnCards.UpdateVisualHand(player, allCardsData, handContainer);
        }
        else if (matchState.Phase == Phase.TeamPhase)
        {
            ResolveRound();
        }
    }

    void ResolveRound()
    {
        blocker.SetActive(true);
        matchState.AdaptStatsToEvent();

        player.CountPoints();
        enemy.CountPoints();

        Debug.Log($"Player Power: {player.Points} Enemy Power: {enemy.Points}");

        if (matchState.Round < 2)
            StartRound();
        else
        {
            if (player.Points > enemy.Points)
                resultText.text = "HAS GANADO";
            else if (enemy.Points > player.Points)
                resultText.text = "EL ENEMIGO GANA";
            else
                resultText.text = "EMPATE";
            resultContainer.SetActive(true);
        }
    }

    public void HastaLuegoMariCarmen()
    {
        Application.Quit();
    }
}
