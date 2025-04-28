using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Scriptables;
using UnityEngine;

namespace Card
{
    public class CardManager : MonoBehaviour
    {
        public List<CardObject> deck;
        public GameObject cardPrefab;
        public Transform cardLayoutParent;

        public List<CardHandler> CurrentHand { get; } = new();
        public bool IsHandUsable { get; private set; }
        private const float CardDrawTime = 0.5f;

        private void Start()
        {
            StartCoroutine(DrawCardsCoroutine());
        }

        private IEnumerator DrawCardsCoroutine()
        {
            IsHandUsable = false;

            yield return new WaitForSeconds(CardDrawTime);

            while (CurrentHand.Count < 5)
            {
                var randomIdx = Random.Range(0, deck.Count);
                var card = deck[randomIdx];
                deck.RemoveAt(randomIdx);

                var cardObject = Instantiate(cardPrefab, cardLayoutParent);
                var cardHandler = cardObject.GetComponent<CardHandler>();
                cardHandler.CardData = card;
                CurrentHand.Add(cardHandler);

                cardObject.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f);
                yield return new WaitForSeconds(CardDrawTime);
            }

            IsHandUsable = true;
        }
    }
}