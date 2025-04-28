using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        public Image healthBar;

        private int _health;
        private int _maxHealth;

        public int Health
        {
            get => _health;
            private set => _health = Mathf.Clamp(value, 0, int.MaxValue);
        }

        public void SetStartingHealth(int health)
        {
            _maxHealth = health;
            Health = health;
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