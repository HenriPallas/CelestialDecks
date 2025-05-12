using JetBrains.Annotations;
using Scriptables;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Random = UnityEngine.Random;

namespace Enemy
{
    [Serializable]
    public class MoveCollection
    {
        public List<CardObject> move;
    }

    public class EnemyManager : ShipManager
    {
        public TextMeshProUGUI handText;

        public List<MoveCollection> possibleOffenseMoves;
        public List<MoveCollection> possibleDefenseMoves;
        private int _lastOffensiveMoveIdx = -1;
        private int _lastDefensiveMoveIdx = -1;

        private List<CardObject> _currentMove = new();
        [CanBeNull] private CardObject _currentMoveEnhanceCard;

        private bool IsOffensive
        {
            get
            {
                var currentHealthPercentage = (float)FullHealth / FullMaxHealth;

                // health is too low to play offensive
                if (currentHealthPercentage <= 0.4f) return false;

                // small chance to still put up some defenses
                return Random.value > 0.2f;
            }
        }

        public void CalculateNextMove(bool gameStart = false)
        {
            if (gameStart || IsOffensive) // First move should always be offensive
                PerformOffensiveMove();
            else
                PerformDefensiveMove();
        }

        [CanBeNull]
        public CardObject GetNextCardFromMove([CanBeNull] out CardObject enhanceCard)
        {
            enhanceCard = _currentMoveEnhanceCard;

            if (!_currentMove.Any()) return null;

            var card = _currentMove[^1];
            _currentMove.RemoveAt(_currentMove.Count - 1);
            UpdateVisibleHand();
            return card;
        }

        private void PerformOffensiveMove()
        {
            var randomIdx = Random.Range(0, possibleOffenseMoves.Count);
            while (randomIdx == _lastOffensiveMoveIdx)
            {
                // Reroll until different move found
                randomIdx = Random.Range(0, possibleOffenseMoves.Count);
            }

            _lastOffensiveMoveIdx = randomIdx;

            var moveCopy = new CardObject[possibleOffenseMoves[randomIdx].move.Count];
            possibleOffenseMoves[randomIdx].move.CopyTo(moveCopy);
            _currentMove = new List<CardObject>(moveCopy);
            UpdateVisibleHand();
        }

        private void PerformDefensiveMove()
        {
            var randomIdx = Random.Range(0, possibleDefenseMoves.Count);
            while (randomIdx == _lastDefensiveMoveIdx)
            {
                // Reroll until different move found
                randomIdx = Random.Range(0, possibleDefenseMoves.Count);
            }

            _lastDefensiveMoveIdx = randomIdx;

            var moveCopy = new CardObject[possibleDefenseMoves[randomIdx].move.Count];
            possibleDefenseMoves[randomIdx].move.CopyTo(moveCopy);
            _currentMove = new List<CardObject>(moveCopy);
            UpdateVisibleHand();
        }

        private void UpdateVisibleHand()
        {
            var moves = _currentMove.Select(c => c.cardName);
            handText.text = string.Join("\n", moves);
        }
    }
}