using UnityEngine;
using System.Collections.Generic;
using Domain.Stats;
using Data;

[System.Serializable]
public struct StatMultiplierEntry
{
	public StatsType Stat;
	public float Multiplier;
}

[CreateAssetMenu(fileName = "NewEventCard", menuName = "Cards/EventCardData")]
public class EventCardData : ScriptableObject, ICardData
{
	[SerializeField] private string cardName;
	public string CardName => cardName;
	[SerializeField] private string habitat;
	public string Habitat => habitat;
	[SerializeField] private string discipline;
	public string Discipline => discipline;
	[SerializeField] private string year;
	public string Year => year;
	[SerializeField] private string modality;
	public string Modality => modality;
	[SerializeField] private Sprite image;
	public Sprite Image => image;
	[SerializeField] private Sprite logo;
	public Sprite Logo => logo;
	[SerializeField] private List<StatMultiplierEntry> multipliers;
	public List<StatMultiplierEntry> Multipliers => multipliers;
}
