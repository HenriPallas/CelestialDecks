using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enemy;
using JetBrains.Annotations;
using Player;
using Scriptables;
using System;
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

        private readonly List<HandCard> _currentHandCards = new();
        public bool IsHandUsable { get; private set; }
        private const float CardDrawTime = 0.5f;
        private const float CardRemoveTime = 0.5f;

        public bool IsHandEmpty => _currentHandCards.Count == 0;
        public bool IsDeckEmpty => deck.Count == 0;

        private PlayerManager _playerManager;
        private EnemyManager _enemyManager;

        private void Start()
        {
            _playerManager = GetComponent<PlayerManager>();
            _enemyManager = GetComponent<EnemyManager>();
            StartCoroutine(DrawCardsCoroutine());
        }

        public List<CardHandler> GetSelectedCards() => _currentHandCards
            .Select(c => c.Card)
            .Where(c => c.IsSelected)
            .ToList();

        private List<int> GetSelectedHandCardIndices() => Enumerable.Range(0, _currentHandCards.Count)
            .Where(idx => _currentHandCards[idx].Card.IsSelected)
            .ToList();

        public void PlayCurrentHand()
        {
            StartCoroutine(PlayHandCoroutine());
        }

        private IEnumerator PlayHandCoroutine()
        {
            IsHandUsable = false;
            var ships = new ShipManager[] { _playerManager, _enemyManager };

            var selectedCardsIndices = GetSelectedHandCardIndices();
            var selectedCards = selectedCardsIndices
                .Select(idx => _currentHandCards[idx].Card).ToArray();

            // Handle card actions
            var enhanceCard = selectedCards.FirstOrDefault(c => c.CardData.cardType == CardType.Enhancement);
            while (selectedCardsIndices.Count > 0)
            {
                yield return new WaitForSeconds(CardRemoveTime);

                var cardIdx = selectedCardsIndices[^1];
                selectedCardsIndices.RemoveAt(selectedCardsIndices.Count - 1);

                // Update indices of selected cards if the currently removed card would shift them
                for (var i = 0; i < selectedCardsIndices.Count; i++)
                {
                    var curIdx = selectedCardsIndices[i];
                    if (curIdx > cardIdx)
                        selectedCardsIndices[i] -= 1;
                }

                var card = _currentHandCards[cardIdx].Card;
                HandleCardAction(_playerManager, _enemyManager, card.CardData, enhanceCard?.CardData);
                RemoveCardFromHand(cardIdx);
                _playerManager.Energy -= card.CardData.energyCost;
            }

            yield return new WaitForSeconds(1.0f);

            // Restore any dodge modifier applied from previous turn
            _enemyManager.RestoreDodge();

            // Enemy AI attack
            var enemyCard = _enemyManager.GetNextCardFromMove(out var enemyEnhanceCard);
            while (enemyCard is not null)
            {
                HandleCardAction(_enemyManager, _playerManager, enemyCard, enemyEnhanceCard);
                yield return new WaitForSeconds(CardRemoveTime);
                enemyCard = _enemyManager.GetNextCardFromMove(out enemyEnhanceCard);
            }

            yield return new WaitForSeconds(1.0f);

            // Turn end
            _playerManager.RestoreDodge();
            foreach (var ship in ships)
            {
                ship.Energy += ship.DeferredEnergy;
                ship.DeferredEnergy = 0;
                ship.Energy += ShipManager.AddedEnergyPerRound;
            }

            _enemyManager.CalculateNextMove();

            StartCoroutine(DrawCardsCoroutine()); // Hand will be made usable from here again
        }

        private void HandleCardAction(ShipManager attackingShip, ShipManager attackedShip, CardObject card,
            [CanBeNull] CardObject enhanceCard)
        {
            switch (card.cardType)
            {
                case CardType.Attack:
                    var damage = card.cardValue;
                    if (enhanceCard is not null)
                    {
                        damage += enhanceCard.cardValue;
                    }

                    attackedShip.Damage(damage);
                    break;
                case CardType.Energy:
                    var energy = card.cardValue;
                    if (enhanceCard is not null)
                    {
                        energy += enhanceCard.cardValue;
                    }

                    // Defer giving the energy to the player only after the cards have been played
                    attackingShip.DeferredEnergy = energy;
                    break;
                case CardType.Shields:
                    var shields = card.cardValue;
                    if (enhanceCard is not null)
                    {
                        shields += enhanceCard.cardValue;
                    }

                    attackingShip.Shield += shields;
                    break;
                case CardType.Dodge:
                    var dodge = 0.2f;
                    if (enhanceCard is not null)
                    {
                        dodge += 0.1f;
                    }

                    attackingShip.ApplyDodge(dodge);
                    break;
                case CardType.Enhancement: // Doesn't really have an "action" associated.
                    break;
                default:
                    throw new NotImplementedException("Card action not implemented");
            }
        }

        private void AddCardToHand(GameObject cardObject, CardObject cardData)
        {
            var cardHandler = cardObject.GetComponent<CardHandler>();
            cardHandler.CardData = cardData;

            _currentHandCards.Add(new HandCard(cardObject, cardHandler));
        }

        private void RemoveCardFromHand(int idx)
        {
            Destroy(_currentHandCards[idx].Object);
            _currentHandCards.RemoveAt(idx);
        }

        private IEnumerator DrawCardsCoroutine()
        {
            IsHandUsable = false;

            yield return new WaitForSeconds(CardDrawTime);

            while (_currentHandCards.Count < 5 && deck.Count > 0)
            {
                var randomIdx = Random.Range(0, deck.Count);
                var card = deck[randomIdx];

                if (_currentHandCards.Any(c => c.Card.CardData.cardType == CardType.Enhancement) &&
                    deck.Any(c => c.cardType != CardType.Enhancement))
                {
                    // if we already have an enhancement card, try to draw something else
                    // only if there are any other cards in the deck besides enhancements
                    while (card.cardType == CardType.Enhancement)
                    {
                        randomIdx = Random.Range(0, deck.Count);
                        card = deck[randomIdx];
                    }
                }

                deck.RemoveAt(randomIdx);

                var cardObject = Instantiate(cardPrefab, cardLayoutParent);
                AddCardToHand(cardObject, card);

                cardObject.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f);
                yield return new WaitForSeconds(CardDrawTime);
            }

            IsHandUsable = true;
        }
    }

    internal readonly struct HandCard
    {
        public readonly GameObject Object;
        public readonly CardHandler Card;

        public HandCard(GameObject obj, CardHandler card)
        {
            Object = obj;
            Card = card;
        }
    }
}