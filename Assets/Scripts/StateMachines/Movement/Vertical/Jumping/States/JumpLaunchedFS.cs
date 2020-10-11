using StateMachines.Movement.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachines.Movement.Vertical.Jumping.States {
    public class JumpLaunchedFS : JumpFS {
        public JumpLaunchedFS(GameObject behaviour, JumpFSM jump, JumpConfig jumpConfig)
            : base(behaviour, jump, jumpConfig) { }

        public override void AcceptJumpInput(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Performed || OutOfJumps()) return;
            Jump.RaiseChangeStateEvent(JumpStates.Launching);
        }

        public override void AcceptDashInput(InputAction.CallbackContext context) {
            if (OutOfDashes()) return;       
            Jump.RaiseChangeStateEvent(JumpStates.Dashing);
        }

        public override void Enter() {
            Rig.gravityScale = Config.lowJumpMultiplier;
            if (Jump.UnitMovementData.moveDir != 0) Behaviour.transform.localScale = new Vector3((int) Jump.UnitMovementData.moveDir, 1, 1);
        }

        public override void Update() {
            Jump.UnitMovementData.jumpTimeLapsed  += Time.deltaTime;
            
            if ((Rig.velocity.y < 0 || Jump.UnitMovementData.jumpTimeLapsed >= Config.jumpDuration) )
                Jump.RaiseChangeStateEvent(JumpStates.Falling);
        }

        public override Vector2 Force() => new Vector2(
            ProvideCappedHorizontalForce(Config.horizontalVelocity, Config.maxVelocity,Jump.UnitMovementData.moveDir, Rig.velocity.x),
            Mathf.Abs(Rig.velocity.y) >= Config.maxVelocity ? 0 : Config.jumpVelocity);
    }
}