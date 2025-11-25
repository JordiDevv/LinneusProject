using UnityEngine;
using System.Collections.Generic;
using Domain.Stats;
using Domain.Rarities;
using Data;

[System.Serializable]
public struct StatEntry
{
    public StatsType StatType;
    public int Value;
}

[CreateAssetMenu(fileName = "NewAnimalCard", menuName = "Cards/AnimalCardData")]
public class AnimalCardData : ScriptableObject, ICardData
{
    [SerializeField] private string cardName;
    public string CardName => cardName;
    [SerializeField] private string scientificName;
    public string ScientificName => scientificName;
    [SerializeField] private RarityType rarity;
    public RarityType Rarity => rarity;
    [SerializeField] private Sprite image;
    public Sprite Image => image;
    [SerializeField] private List<StatEntry> stats;
    public List<StatEntry> Stats => stats;
}
