using Scriptables;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        public Image healthBar;

        private int _health;
        private int _maxHealth;
        private int _shield;
        private int _maxShield;
        private int _dodge;
        public HullObject hull;

        public int Health
        {
            get => _health;
            private set => _health = Mathf.Clamp(value, 0, int.MaxValue);
        }

        public int Shield
        {
            get => _shield;
            private set => _shield = Mathf.Clamp(value, 0, int.MaxValue);
        }

        public int Dodge
        {
            get => _dodge;
            private set => _dodge = Mathf.Clamp(value, 0, 1);
        }

        public void SetStartingHealth(int health)
        {
            _maxHealth = health;
            Health = health;
        }

        public void SetStartingShield(int health)
        {
            _maxShield = health;
            Shield = health;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            var healthPercentage = (float)_health / _maxHealth;
            var barScale = new Vector3(healthPercentage, 1, 1);
            healthBar.transform.localScale = barScale;
        }
    }
}
