using StateMachines.Movement.Models;
using UnityEngine;

namespace StateMachines.Movement.Vertical.Jumping {
    public static class JumpConfigExtensions {
        public static JumpConfig CreateInstance(this JumpConfig config) {
            var instance = ScriptableObject.CreateInstance<JumpConfig>();
            instance.jumpDuration = config.jumpDuration;
            instance.jumpVelocity = config.jumpVelocity;
            instance.maxVelocity = config.maxVelocity;
            instance.maxDashVelocity = config.maxDashVelocity;
            instance.fallMultiplier = config.fallMultiplier;
            instance.lowJumpMultiplier = config.lowJumpMultiplier;
            instance.groundedLinearDrag = config.groundedLinearDrag;
            instance.aerialLinearDrag = config.aerialLinearDrag;
            instance.dashDuration = config.dashDuration;
            instance.horizontalVelocity = config.horizontalVelocity;
            instance.dashHorizontalVelocity = config.dashHorizontalVelocity;
            instance.maxJumps = config.maxJumps;
            instance.jumpsLeft = config.jumpsLeft;
            instance.maxDashes = config.maxDashes;
            instance.dashesLeft = config.dashesLeft;
            return instance;
        }
    }
}