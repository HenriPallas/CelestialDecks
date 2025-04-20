using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Card
{
    public class CardEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private GameObject _highlightPanel;
        public event EventHandler CardClicked;

        private void Start()
        {
            _highlightPanel = gameObject.transform.Find("Highlight").gameObject;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _highlightPanel.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _highlightPanel.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                CardClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}