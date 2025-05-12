using System;
using TMPro;

namespace Player
{
    public class PlayerManager : ShipManager
    {
        public TextMeshProUGUI energyText;

        protected override void UpdateUI()
        {
            energyText.text = Energy.ToString();
            base.UpdateUI();
        }
    }
}