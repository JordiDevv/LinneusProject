using UnityEngine;

namespace Data
{
    public interface ICardData
    {
        string CardName { get; }
        Sprite Image { get; }
    }
}
