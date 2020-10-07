using UnityEngine;

namespace StateMachines.State {
    public class UnitStateStore : MonoBehaviour {
        [SerializeField] public UnitState store = new UnitState();
    }
}