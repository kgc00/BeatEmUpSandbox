using System;
using UnityEngine;

namespace StateMachines.Movement.Vertical.Jumping {
    [CreateAssetMenu(fileName = "JumpConfig", menuName = "Model/Jump", order = 0)]
    [Serializable]
    public class JumpConfig : ScriptableObject {
        [SerializeField] public float jumpDuration;
        [SerializeField] public float jumpVelocity;
        [SerializeField] public float maxVelocity;
        [SerializeField] public float maxDashVelocity;
        [SerializeField] public float fallMultiplier;
        [SerializeField] public float lowJumpMultiplier;
        [SerializeField] public float groundedLinearDrag;
        [SerializeField] public float aerialLinearDrag;
        [SerializeField] public int maxJumps;
        [SerializeField] public int jumpsLeft;
        [SerializeField] public int maxDashes;
        [SerializeField] public int dashesLeft;
        [SerializeField] public float dashDuration;
        [SerializeField] public float horizontalVelocity;
        [SerializeField] public float dashHorizontalVelocity;
    }
}