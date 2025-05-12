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
        public TextMeshProUGUI descriptionText;

        private CardEventHandler _cardEventHandler;

        public CardObject CardData { get; set; }
        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            private set
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
            descriptionText.text = CardData.cardDescription;

            _cardEventHandler = gameObject.AddComponent<CardEventHandler>();
            _cardEventHandler.CardClicked += (_, _) => { IsSelected = !IsSelected; };

            var layout = GetComponentInParent<HorizontalLayoutNotified>();
            layout.onLayoutChanged.AddListener(() =>
            {
                // Make sure the position of selected cards stays the same after layout refresh
                if (_isSelected)
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                        gameObject.transform.position.y + SelectedCardPosition, gameObject.transform.position.z);
            });
        }
    }
}