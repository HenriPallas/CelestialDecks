using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        public TextMeshProUGUI energyText;

        public const int AddedEnergyPerRound = 4;
        private const int MaxEnergy = 8;
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
    }
}