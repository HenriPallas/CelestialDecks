using Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipManager : MonoBehaviour
{
    public Image healthBar;
    public Image shieldBar;
    public TextMeshProUGUI healthText;

    // ship stats
    private int _energy;
    private int _health;
    private int _shield;
    private float _dodge;
    private float _lastDodge;

    public HullObject hull;

    public int maxEnergy = 8;
    public const int AddedEnergyPerRound = 4;
    public int startingEnergy = 5;

    public float startingDodge = 0.4f;

    public int maxHealth = 20;
    public int maxShield = 10;

    public int Energy
    {
        get => _energy;
        set
        {
            _energy = Mathf.Clamp(value, 0, maxEnergy);
            UpdateUI();
        }
    }

    public int Health
    {
        get => _health;
        set
        {
            _health = Mathf.Clamp(value, 0, maxHealth);
            UpdateUI();
        }
    }

    public int Shield
    {
        get => _shield;
        set
        {
            _shield = Mathf.Clamp(value, 0, maxShield);
            UpdateUI();
        }
    }

    protected float Dodge
    {
        get => _dodge;
        set
        {
            _dodge = Mathf.Clamp(value, 0f, 1f);
            UpdateUI();
        }
    }

    // Set start stats

    private void Start()
    {
        Energy = startingEnergy;
        Health = maxHealth;
        Shield = maxShield;
        Dodge = startingDodge;
        _lastDodge = startingDodge;
    }

    // Remove from stat value

    public void Damage(int damage)
    {
        // Handle the possibility of dodging the attack
        if (Random.value <= Dodge) return;
        
        // Otherwise inflict damage
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

        UpdateUI();
    }

    public void ApplyDodge(float dodge)
    {
        Dodge += dodge;
    }

    public void RestoreDodge()
    {
        Dodge = _lastDodge;
    }

    protected virtual void UpdateUI()
    {
        healthText.text = $"{_health}/{maxHealth} + {_shield}/{maxShield}";

        // Update the health and shield bars
        var healthPercentage = (float)_health / maxHealth;
        var healthBarScale = new Vector3(healthPercentage, 1, 1);
        healthBar.transform.localScale = healthBarScale;

        var shieldPercentage = (float)_shield / maxShield;
        var percentageOfHealth = (float)_shield / maxHealth;
        var shieldBarScale = new Vector3(shieldPercentage * percentageOfHealth, 1, 1);
        shieldBar.transform.localScale = shieldBarScale;
    }
}