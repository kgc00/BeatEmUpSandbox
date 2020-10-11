using System;
using UnityEngine;

namespace StateMachines.State {
    public class UnitDataStore : MonoBehaviour {
        [SerializeField] public UnitMovementData store = new UnitMovementData();
    }
}