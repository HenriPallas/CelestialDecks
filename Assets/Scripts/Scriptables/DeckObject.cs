using System.Collections.Generic;
using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "New Deck", menuName = "Scriptables/Deck")]
    public class DeckObject : ScriptableObject
    {
        public string deckName;
        public string deckDescription;
        public List<float> deckMultipliers;
        public List<CardObject> deckCards;
        public bool isUnlocked;
        public bool isLockedByDefault;
        public int unlockCost;
    }
}
