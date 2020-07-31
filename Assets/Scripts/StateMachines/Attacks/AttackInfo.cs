using System;
using UnityEngine;

namespace StateMachines.Attacks {
    [Serializable]
    [CreateAssetMenu(fileName = "Attack Info", menuName = "Model/Attack", order = 0)]
    public class AttackInfo : ScriptableObject {
        [SerializeField] public float startupTime;
        [SerializeField] public float cooldownTime;
    }
}