using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        public Image healthBar;
        public Image shieldBar;
        public TextMeshProUGUI energyText;
        public TextMeshProUGUI healthText;

        public const int AddedEnergyPerRound = 4;
        private const int MaxEnergy = 8;
        private int MaxHealth = 100;
        private int MaxShield = 50;
        private int _energy;
        private int _health;
        private int _shield;
        private float _dodge;

        public int Energy
        {
            get => _energy;
            set => _energy = Mathf.Clamp(value, 0, MaxEnergy);
        }

        public int Health
        {
            get => _health;
            set => _health = Mathf.Clamp(value, 0, MaxHealth);
        }

        public int Shield
        {
            get => _shield;
            set => _shield = Mathf.Clamp(value, 0, MaxShield);
        }

        public float Dodge
        {
            get => _dodge;
            set => _dodge = Mathf.Clamp(value, 0f, 1f);
        }

        private void Update()
        {
            energyText.text = Energy.ToString();
            healthText.text = Health.ToString() + "/" + MaxHealth.ToString() + " + " + Shield.ToString() + "/" + MaxShield.ToString();
        }

        public void SetHealth(int value)
        {
            Health = value;
        }

        public void SetShield(int value)
        {
            Shield = value;
        }

        public void SetDodge(float value)
        {
            Dodge = value;
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

        public void UpdateBars()
        {
            var healthPercentage = (float)Health / MaxHealth;
            var shieldPercentage = (float)Shield / MaxShield;
            var healthBarScale = new Vector3(healthPercentage, 1, 1);
            var shieldBarScale = new Vector3(shieldPercentage, 1, 1);
            healthBar.transform.localScale = healthBarScale;
            shieldBar.transform.localScale = shieldBarScale;
        }
    }
}
