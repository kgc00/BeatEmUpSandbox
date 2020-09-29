using System;

namespace StateMachines.Movement.Models {
    [Serializable]
    public class MovementValues {
        public float moveDir;
        public int jumpsLeft;
        public int dashesLeft;
        public float timeLeft;

        public MovementValues() { }

        public MovementValues(MovementValues mv) {
            moveDir = mv.moveDir;
            jumpsLeft = mv.jumpsLeft;
            dashesLeft = mv.dashesLeft;
            timeLeft = mv.timeLeft;
        }

        public MovementValues(float moveDir, int jumpsLeft, int dashesLeft, float timeLeft) {
            this.moveDir = moveDir;
            this.jumpsLeft = jumpsLeft;
            this.dashesLeft = dashesLeft;
            this.timeLeft = timeLeft;
        }
    }
}