using UnityEngine;

namespace Common {
    public static class SandboxUtils {
        public static bool IsForwardMovement(float moveDir, float rigX) => Mathf.Sign(moveDir) == Mathf.Sign(rigX);
    }
}