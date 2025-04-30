using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Scriptables/Card")]
    public class CardObject : ScriptableObject
    {
        public string cardName;
        public string cardDescription;
        public int energyCost;
        public CardType cardType;
        public int cardValue;
        public string[] cardEffects;
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
