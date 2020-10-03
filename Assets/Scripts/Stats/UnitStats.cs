using System;
using UnityEngine;

namespace Stats {
    [Serializable]
    [CreateAssetMenu(fileName = "Unit Stats", menuName = "Model/Unit Stats", order = 0)]
    public class UnitStats : ScriptableObject {
        [SerializeField] public int maxHealth;
    }
}