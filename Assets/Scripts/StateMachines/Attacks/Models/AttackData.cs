using System;
using TypeReferences;
using UnityEngine;

namespace StateMachines.Attacks.Models {
    [Serializable]
    public class AttackData {
        [SerializeField] public GameObject HitboxObject;
        [SerializeField] public TypeReference AttckFS;
    }
}