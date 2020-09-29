namespace StateMachines.Network {
    public static class NetworkedEventCodes {
        public const byte ChangeAttackStateEventCode = 1;
        public const byte ChangeJumpStateEventCode = 2;
        public const byte ChangeRunStateEventCode = 3;
        public const byte RunCollisionEventCode = 4;
        public const byte JumpCollisionEventCode = 5;
        public const byte SetMovementValuesEventCode = 6;
        public const byte SetMovementDirEventCode = 7;
    }
}