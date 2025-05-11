using Scriptables;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        public Image healthBar;
        public Image shieldBar;
        public TextMeshProUGUI healthText;

        private const int MaxHealth = int.MaxValue;
        private const int MaxShield = int.MaxValue;

        private int _health;
        private int _maxHealth;
        private int _shield;
        private int _maxShield;
        private float _dodge;

        public HullObject hull;

        public int Health
        {
            get => _health;
            private set => _health = Mathf.Clamp(value, 0, MaxHealth);
        }

        public int Shield
        {
            get => _shield;
            private set => _shield = Mathf.Clamp(value, 0, MaxShield);
        }

        public float Dodge
        {
            get => _dodge;
            private set => _dodge = Mathf.Clamp(value, 0f, 1f);
        }

        private void Update()
        {
            healthText.text = $"{_health}/{_maxHealth} + {_shield}/{_maxShield}";
        }

        // Set start stats

        public void SetStartingHealth(int health)
        {
            Debug.Log(health);
            _maxHealth = health;
            Health = health;
            UpdateBars();
        }

        public void SetStartingShield(int shield)
        {
            Debug.Log(shield);
            _maxShield = shield;
            Shield = shield;
            UpdateBars();
        }

        public void SetStartingDodge(float dodge)
        {
            Dodge = dodge;
            UpdateBars();
        }

        // Set stat values

        public void SetHealth(int value)
        {
            Health = value;
            UpdateBars();
        }

        public void SetShield(int value)
        {
            Shield = value;
            UpdateBars();
        }

        public void SetDodge(float value)
        {
            Dodge = value;
            UpdateBars();
        }

        // Add to stat values

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

        public void AddDodge(float value)
        {
            Dodge += value;
            UpdateBars();
        }

        // Remove from stat value

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

        public void RemoveHealth(int value)
        {
            Health -= value;
            UpdateBars();
        }

        public void RemoveShield(int value)
        {
            Shield -= value;
            UpdateBars();
        }

        public void RemoveDodge(float value)
        {
            Dodge -= value;
            UpdateBars();
        }

        // Update UI

        public void UpdateBars()
        {
            if (_health > 0 && _maxHealth > 0)
            {
                var healthPercentage = (float)_health / _maxHealth;
                var healthBarScale = new Vector3(healthPercentage, 1, 1);
                healthBar.transform.localScale = healthBarScale;
            }

            if (_shield > 0 && _maxShield > 0)
            {
                var shieldPercentage = (float)_shield / _maxShield;
                var percentageOfHealth = (float)_shield / _maxHealth;
                var shieldBarScale = new Vector3(shieldPercentage * percentageOfHealth, 1, 1);
                shieldBar.transform.localScale = shieldBarScale;
            }
        }
    }
}
