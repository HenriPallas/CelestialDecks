using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Scriptables/Card")]
    public class CardObject : ScriptableObject
    {
        public string cardName;
        public int energyCost;
        public CardType cardType;
    }

    public enum CardType
    {
        Attack,
        Shields,
        Dodge,
        Enhancement,
        Energy,
    }
}