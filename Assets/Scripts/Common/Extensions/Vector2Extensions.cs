using UnityEngine;

namespace Common.Extensions {
    public static class Vector2Extensions {
        public static Vector3 Vector3(this Vector2 v) => new Vector3(v.x, v.y, 0f);
    }
}