using DG.Tweening;
using Scriptables;
using TMPro;
using UI.Card;
using UnityEngine;

namespace Card
{
    public class CardHandler : MonoBehaviour
    {
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI energyCostText;

        private CardEventHandler _cardEventHandler;

        public CardObject CardData { get; set; }
        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                var offset = value ? SelectedCardPosition : -SelectedCardPosition;

                gameObject.transform.DOMoveY(offset + gameObject.transform.position.y, 0.15f);
                _isSelected = value;
            }
        }

        private const float SelectedCardPosition = 45f;

        private void Start()
        {
            nameText.text = CardData.name;
            energyCostText.text = CardData.energyCost.ToString();

            _cardEventHandler = gameObject.AddComponent<CardEventHandler>();
            _cardEventHandler.CardClicked += (_, _) => { IsSelected = !IsSelected; };
        }
    }
}