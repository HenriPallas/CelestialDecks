using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        public TextMeshProUGUI energyText;

        private const int AddedEnergyPerRound = 4;
        private const int MaxEnergy = 7;
        private int _energy;

        public int Energy
        {
            get => _energy;
            set => _energy = Mathf.Clamp(value, 0, MaxEnergy);
        }

        private void Update()
        {
            energyText.text = Energy.ToString();
        }

        public void UpdateEnergyFromHand(int energyCost)
        {
            Energy = Energy - energyCost + AddedEnergyPerRound;
        }
    }
}