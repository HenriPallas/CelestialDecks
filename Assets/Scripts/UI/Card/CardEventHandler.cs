using System;
using Card;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Card
{
    public class CardEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private GameObject _highlightPanel;
        private CardManager _cardManager;
        public event EventHandler CardClicked;

        private void Start()
        {
            _cardManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CardManager>();
            _highlightPanel = gameObject.transform.Find("Highlight").gameObject;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_cardManager.IsHandUsable) return;
            _highlightPanel.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_cardManager.IsHandUsable) return;
            _highlightPanel.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_cardManager.IsHandUsable && eventData.button == PointerEventData.InputButton.Left)
                CardClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}