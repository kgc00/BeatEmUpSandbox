using System;
using StateMachines.Attacks.States;
using TypeReferences;
using UnityEngine;

namespace StateMachines.Attacks.Models {
    [Serializable]
    public class AttackData {
        [SerializeField] public GameObject HitboxObject;
        [SerializeField, Inherits(typeof(AttackFS))] public TypeReference AttckFS;
    }
}