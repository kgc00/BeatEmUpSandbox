using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General {
    public abstract class ForceStrategy : ScriptableObject {
        public abstract IEnumerator Execute(Collider2D other, Rigidbody2D rigidBody,
            Transform forceComponentTransform);
    }
}