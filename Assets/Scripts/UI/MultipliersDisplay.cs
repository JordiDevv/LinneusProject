using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Domain.CardsSystem.Definitions;

public class MultipliersDisplay : MonoBehaviour
{
    public void SetMultipliers(EventCard eventCard, List<GameObject> multipliersGO, List<Sprite> multipliersImages)
    {
        var multipliers = eventCard.Discipline.Multipliers.Multipliers;

        var sortedKeys = multipliers
            .Where(kv => Mathf.Approximately(kv.Value, 2f))
            .Concat(multipliers.Where(kv => Mathf.Approximately(kv.Value, 1f)))
            .Select(kv => kv.Key)
            .ToList();

        int i = 0;
        foreach (var stat in sortedKeys)
        {
            if (i >= multipliersGO.Count) break;

            string statName = stat.ToString();

            Sprite matchingSprite = multipliersImages
                .FirstOrDefault(sprite => sprite.name.Contains(statName));

            if (matchingSprite == null)
            {
                Debug.LogWarning($"Not found '{statName}'");
                continue;
            }

            Transform child = multipliersGO[i].transform.GetChild(0);
            Image img = child.GetComponent<Image>();
            if (img != null)
            {
                img.sprite = matchingSprite;
                img.enabled = true;
                i++;
            }
        }

        for (; i < multipliersGO.Count; i++)
        {
            multipliersGO[i].SetActive(false);
        }
    }
}
