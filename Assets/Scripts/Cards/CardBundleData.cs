using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "New CardBundleData", menuName = "Card Bundle Data", order = 10)]
    public class CardBundleData : ScriptableObject
    {
        [field: SerializeField] public CardData[] CardData { get; private set; }
    }
}