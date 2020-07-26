using UnityEngine;

namespace Common.Extensions {
    public static class Vector3Extensions {
        public static Vector2 Vector2(this Vector3 v) => new Vector2(v.x, v.y);
    }
}