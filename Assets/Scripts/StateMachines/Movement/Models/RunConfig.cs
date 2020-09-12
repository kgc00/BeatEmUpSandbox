using System;
using UnityEngine;

namespace StateMachines.Movement {
    [CreateAssetMenu(fileName = "RunConfig", menuName = "Model/Run", order = 0)]
    [Serializable]
    public class RunConfig : ScriptableObject {
        [SerializeField] public float runVelocity;
        [SerializeField] public float maxVelocity;
        [SerializeField] public float dashDuration;
        [SerializeField] public float dashVelocity;
    }
}