using StateMachines.Attacks;
using StateMachines.Attacks.States;
using StateMachines.Movement.Horizontal.Run;
using StateMachines.Movement.Horizontal.Run.States;
using StateMachines.Movement.Models;
using StateMachines.Movement.Vertical.Jumping;
using StateMachines.Movement.Vertical.Jumping.States;
using IdleFS = StateMachines.Attacks.States.IdleFS;
using LockedFS = StateMachines.Movement.Vertical.Jumping.States.LockedFS;

namespace StateMachines.Network {
    public static class StateFactory {
        public static AttackFS AttackFSFromEnum(AttackStates state, AttackFSM fsm) {
            if (state == AttackStates.Idle) return new IdleFS(fsm.gameObject, fsm, fsm.kit, fsm.UnitState);
            if (state == AttackStates.GroundedNeutralOne)
                return new GroundedNeutralOneFS(fsm.gameObject, fsm, fsm.kit, fsm.UnitState);
            if (state == AttackStates.GroundedNeutralTwo)
                return new GroundedNeutralTwoFS(fsm.gameObject, fsm, fsm.kit, fsm.UnitState);
            if (state == AttackStates.GroundedNeutralThree)
                return new GroundedNeutralThreeFS(fsm.gameObject, fsm, fsm.kit, fsm.UnitState);
            if (state == AttackStates.GroundedForwardAttack)
                return new GroundedForwardAttackFS(fsm.gameObject, fsm, fsm.kit, fsm.UnitState);
            if (state == AttackStates.GroundedUpAttack)
                return new GroundedUpAttackFS(fsm.gameObject, fsm, fsm.kit, fsm.UnitState);
            return null;
        }

        public static JumpFS JumpFSFromEnum(JumpStates state, JumpFSM fsm) {
            if (state == JumpStates.Grounded) return new JumpGroundedFS(fsm.Behaviour, fsm, fsm.Config);
            if (state == JumpStates.Launching) return new JumpLaunchingFS(fsm.Behaviour, fsm, fsm.Config);
            if (state == JumpStates.Launched) return new JumpLaunchedFS(fsm.Behaviour, fsm, fsm.Config);
            if (state == JumpStates.Falling) return new JumpFallingFS(fsm.Behaviour, fsm, fsm.Config);
            if (state == JumpStates.Dashing) return new JumpDashingFS(fsm.Behaviour, fsm, fsm.Config);
            if (state == JumpStates.Locked) return new LockedFS(fsm.Behaviour, fsm, fsm.Config);
            return null;
        }

        public static RunFS RunFSFromEnum(RunStates state, RunFSM fsm) {
            if (state == RunStates.Idle)
                return new Movement.Horizontal.Run.States.IdleFS(fsm.Behaviour, fsm.Config, fsm);
            if (state == RunStates.Moving) return new MovingFS(fsm.Behaviour, fsm.Config, fsm);
            if (state == RunStates.Dash) return new DashFS(fsm.Behaviour, fsm.Config, fsm);
            if (state == RunStates.Locked)
                return new Movement.Horizontal.Run.States.LockedFS(fsm.Behaviour, fsm.Config, fsm);
            return null;
        }
    }
}