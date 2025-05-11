using System;
using TMPro;

namespace Player
{
    public class PlayerManager : ShipManager
    {
        public TextMeshProUGUI energyText;
        public TextMeshProUGUI dodgeText;

        protected override void UpdateUI()
        {
            energyText.text = Energy.ToString();
            dodgeText.text = $"{(int)(Math.Round(Dodge, 2) * 100)}%";
            base.UpdateUI();
        }
    }
}