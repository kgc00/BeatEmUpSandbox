using System;

namespace StateMachines.State {
    [Serializable]
    public class UnitState {
        public float moveDir;
        public int jumpsLeft;
        public float jumpTimeLapsed;
        public int dashesLeft;
        public float dashTimeLapsed;
        public bool touchingWall;
        public bool touchingGround;
    
        public UnitState() { }

        public UnitState(UnitState mv) {
            moveDir = mv.moveDir;
            jumpsLeft = mv.jumpsLeft;
            jumpTimeLapsed = mv.jumpTimeLapsed;
            dashesLeft = mv.dashesLeft;
            dashTimeLapsed = mv.dashTimeLapsed;
            touchingWall = mv.touchingWall;
            touchingGround = mv.touchingGround;
        }

        public UnitState(float moveDir, int jumpsLeft, float jumpTimeLapsed, int dashesLeft, float dashTimeLapsed,
            bool touchingWall, bool touchingGround) {
            this.moveDir = moveDir;
            this.jumpsLeft = jumpsLeft;
            this.jumpTimeLapsed = jumpTimeLapsed;
            this.dashesLeft = dashesLeft;
            this.dashTimeLapsed = dashTimeLapsed;
            this.touchingWall = touchingWall;
            this.touchingGround = touchingGround;
        }
    }
}