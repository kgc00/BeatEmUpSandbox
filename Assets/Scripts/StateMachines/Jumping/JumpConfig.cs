using System;
using UnityEngine;

namespace StateMachines.Jumping {
    [CreateAssetMenu(fileName = "JumpConfig", menuName = "Model/Jump", order = 0)]
    [Serializable]
    public class JumpConfig : ScriptableObject {
        [SerializeField] public float jumpDuration;
        [SerializeField] public float jumpVelocity;
        [SerializeField] public float fallMultiplier;
        [SerializeField] public float lowJumpMultiplier;
    }
}