using System;
using UnityEngine;

namespace StateMachines.Movement.Vertical.Jumping {
    [CreateAssetMenu(fileName = "JumpConfig", menuName = "Model/Jump", order = 0)]
    [Serializable]
    public class JumpConfig : ScriptableObject {
        [SerializeField] public float jumpDuration;
        [SerializeField] public float jumpVelocity;
        [SerializeField] public float maxVelocity;
        [SerializeField] public float fallMultiplier;
        [SerializeField] public float lowJumpMultiplier;
        [SerializeField] public float groundedLinearDrag;
        [SerializeField] public float aerialLinearDrag;
    }
}