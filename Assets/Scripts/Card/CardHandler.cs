using Scriptables;
using TMPro;
using UI.Card;
using UnityEngine;
using UnityEngine.UI;

namespace Card
{
    public class CardHandler : MonoBehaviour
    {
        public CardObject cardData;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI energyCostText;

        private CardEventHandler _cardEventHandler;

        private bool _isSelected;
        private Vector3 _originalCardPosition;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                var offsetPos = value ? new Vector3(0, SelectedCardPosition, 0) : new Vector3(0, 0, 0);

                gameObject.transform.position = _originalCardPosition + offsetPos;
                _isSelected = value;
            }
        }

        private const float SelectedCardPosition = 45f;

        private void Start()
        {
            _originalCardPosition = gameObject.transform.position;

            nameText.text = cardData.name;
            energyCostText.text = cardData.energyCost.ToString();

            _cardEventHandler = gameObject.AddComponent<CardEventHandler>();
            _cardEventHandler.CardClicked += (_, _) => { IsSelected = !IsSelected; };
        }
    }
}