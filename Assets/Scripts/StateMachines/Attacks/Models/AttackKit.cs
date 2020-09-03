using System;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachines.Attacks.Models {
    [Serializable]
    public class AttackKit {
        [SerializeField] public List<AttackData> attacks;
    }
}