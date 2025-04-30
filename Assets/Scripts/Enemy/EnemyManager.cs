using Scriptables;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        public Image healthBar;
        public Image shieldBar;

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
            UpdateBars();
        }

        public void SetStartingShield(int health)
        {
            _maxShield = health;
            Shield = health;
            UpdateBars();
        }

        public void HealDamage(int value)
        {
            Health += value;
            UpdateBars();
        }

        public void AddShield(int value)
        {
            Shield += value;
            UpdateBars();
        }

        public void TakeDamage(int damage)
        {
            if (Shield > 0)
            {
                if (Shield >= damage)
                {
                    Shield -= damage;
                }
                else
                {
                    damage -= Shield;
                    Shield = 0;
                    Health -= damage;
                }
            }
            else
            {
                Health -= damage;
            }
            UpdateBars();
        }

        public void UpdateBars()
        {
            var healthPercentage = (float)_health / _maxHealth;
            var healthBarScale = new Vector3(healthPercentage, 1, 1);
            healthBar.transform.localScale = healthBarScale;
            var shieldPercentage = (float)_shield / _maxShield;
            var shieldBarScale = new Vector3(shieldPercentage, 1, 1);
            shieldBar.transform.localScale = shieldBarScale;
        }
    }
}
