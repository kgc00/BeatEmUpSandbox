using StateMachines.Attacks;
using StateMachines.Attacks.States;

namespace StateMachines.Network {
    public static class AttackStateFactory {
        public static AttackFS FSFromEnum(AttackStates state, AttackFSM fsm) {
            if(state == AttackStates.Idle) return new IdleFS(fsm.gameObject, fsm, fsm.kit);
            if(state == AttackStates.PunchOne) return new PunchOneFS(fsm.gameObject, fsm, fsm.kit);
            if(state == AttackStates.PunchTwo) return new PunchTwoFS(fsm.gameObject, fsm, fsm.kit);
            if(state == AttackStates.PunchThree) return new PunchThreeFS(fsm.gameObject, fsm, fsm.kit);
            return null;
        }
    }
}