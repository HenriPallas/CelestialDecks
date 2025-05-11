using System.Collections.Generic;
using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "New Hull", menuName = "Scriptables/Hull")]
    public class HullObject : ScriptableObject
    {
        public string hullName;
        public string hullDescription;
        public List<float> hullMultipliers;
    }
}
