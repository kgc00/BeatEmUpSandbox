using System;

namespace StateMachines.Movement.Models {
    [Serializable]
    public class MovementValues {
        public float moveDir;
        public int jumpsLeft;
        public float jumpTimeLapsed;
        public int dashesLeft;
        public float dashTimeLapsed;

        public MovementValues() { }

        public MovementValues(MovementValues mv) {
            moveDir = mv.moveDir;
            jumpsLeft = mv.jumpsLeft;
            jumpTimeLapsed = mv.jumpTimeLapsed;
            dashesLeft = mv.dashesLeft;
            dashTimeLapsed = mv.dashTimeLapsed;
        }

        public MovementValues(float moveDir, int jumpsLeft, float jumpTimeLapsed, int dashesLeft, float dashTimeLapsed) {
            this.moveDir = moveDir;
            this.jumpsLeft = jumpsLeft;
            this.jumpTimeLapsed = jumpTimeLapsed;
            this.dashesLeft = dashesLeft;
            this.dashTimeLapsed = dashTimeLapsed;
        }
    }
}