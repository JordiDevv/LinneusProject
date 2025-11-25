using UnityEngine;
using Domain.Stats;
using Data;

[CreateAssetMenu(fileName = "NewMutationCard", menuName = "Cards/MutationCardData")]
public class MutationCardData : ScriptableObject, ICardData
{
    [SerializeField] private string cardName;
    public string CardName => cardName;
    [SerializeField] private string taxonName;
    public string TaxonName => taxonName;
    [SerializeField] private StatsType stat;
    public StatsType Stat => stat;
    [SerializeField] private int value;
    public int Value => value;
    [SerializeField] private bool allStats;
    public bool AllStats => allStats;
    [SerializeField] private bool team;
    public bool Team => team;
    [SerializeField] private Sprite image;
    public Sprite Image => image;
}
