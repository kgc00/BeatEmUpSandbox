using StateMachines.Attacks;
using StateMachines.Attacks.States;
using StateMachines.Movement.Horizontal.Run;
using StateMachines.Movement.Models;
using StateMachines.Movement.Vertical.Jumping;
using IdleFS = StateMachines.Attacks.States.IdleFS;
using LockedFS = StateMachines.Movement.Vertical.Jumping.LockedFS;

namespace StateMachines.Network {
    public static class StateFactory {
        public static AttackFS AttackFSFromEnum(AttackStates state, AttackFSM fsm) {
            if(state == AttackStates.Idle) return new IdleFS(fsm.gameObject, fsm, fsm.kit);
            if(state == AttackStates.PunchOne) return new PunchOneFS(fsm.gameObject, fsm, fsm.kit);
            if(state == AttackStates.PunchTwo) return new PunchTwoFS(fsm.gameObject, fsm, fsm.kit);
            if(state == AttackStates.PunchThree) return new PunchThreeFS(fsm.gameObject, fsm, fsm.kit);
            return null;
        }
        
        public static JumpFS JumpFSFromEnum(JumpStates state, JumpFSM fsm, float moveDir = 0f, float timeLapsed = 0f) {
            if(state == JumpStates.Grounded) return new JumpGroundedFS(fsm.Behaviour, fsm, fsm.Config, moveDir);
            if(state == JumpStates.Launching) return new JumpLaunchingFS(fsm.Behaviour, fsm, fsm.Config, moveDir);
            if(state == JumpStates.Launched) return new JumpLaunchedFS(fsm.Behaviour, fsm, fsm.Config, moveDir, timeLapsed);
            if(state == JumpStates.Falling) return new JumpFallingFS(fsm.Behaviour, fsm, fsm.Config, moveDir);
            if(state == JumpStates.Dashing) return new JumpDashingFS(fsm.Behaviour, fsm, fsm.Config, moveDir);
            if(state == JumpStates.Locked) return new LockedFS(fsm.Behaviour, fsm, fsm.Config, moveDir);
            return null;
        }
        
        public static RunFS RunFSFromEnum(RunStates state, RunFSM fsm, float moveDir = 0f) {
            if(state == RunStates.Idle) return new Movement.Horizontal.Run.IdleFS(fsm.Behaviour, fsm.Config, fsm, moveDir);
            if(state == RunStates.Moving) return new Movement.Horizontal.Run.MovingFS(fsm.Behaviour, fsm.Config, fsm, moveDir);
            if(state == RunStates.Dash) return new Movement.Horizontal.Run.DashFS(fsm.Behaviour, fsm.Config, fsm, moveDir);
            if(state == RunStates.Locked) return new Movement.Horizontal.Run.LockedFS(fsm.Behaviour, fsm.Config, fsm, moveDir);
            return null;
        }
    }
}