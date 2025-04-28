using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enemy;
using Player;
using Scriptables;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Card
{
    [RequireComponent(typeof(PlayerManager))]
    [RequireComponent(typeof(EnemyManager))]
    public class CardManager : MonoBehaviour
    {
        public List<CardObject> deck;
        public GameObject cardPrefab;
        public Transform cardLayoutParent;

        public List<CardHandler> CurrentHand { get; } = new();
        public bool IsHandUsable { get; private set; }
        private const float CardDrawTime = 0.5f;

        private PlayerManager _playerManager;
        private EnemyManager _enemyManager;

        private void Start()
        {
            _playerManager = GetComponent<PlayerManager>();
            _enemyManager = GetComponent<EnemyManager>();
            StartCoroutine(DrawCardsCoroutine());
        }

        public void PlayCurrentHand()
        {
            var selectedCards = CurrentHand.Where(card => card.IsSelected).ToList();
            var isEnhanced = selectedCards.Any(card => card.CardData.cardType == CardType.Enhancement);
            selectedCards.ForEach(card => HandleCardAction(card.CardData.cardType, isEnhanced));

            var energyCost = CurrentHand.Where(card => card.IsSelected)
                .Sum(card => card.CardData.energyCost);
            _playerManager.UpdateEnergyFromHand(energyCost);

            selectedCards.ForEach(card => card.IsSelected = false);
        }

        private void HandleCardAction(CardType cardType, bool isEnhanced)
        {
            switch (cardType)
            {
                case CardType.Attack:
                    var damage = isEnhanced ? 3 : 2;
                    _enemyManager.TakeDamage(damage);
                    break;
                case CardType.Energy:
                    _playerManager.Energy += 2;
                    break;
                case CardType.Shields:
                case CardType.Dodge:
                case CardType.Enhancement:
                default: break;
            }
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