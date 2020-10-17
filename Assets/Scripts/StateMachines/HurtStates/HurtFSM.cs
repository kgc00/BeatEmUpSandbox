using Photon.Pun;
using StateMachines.HurtStates.States;
using StateMachines.Interfaces;
using StateMachines.Network;

namespace StateMachines.HurtStates {
    public class HurtFSM : MonoBehaviourPun, IChangeStatePun<Model.HurtStates> {
        public HurtFS State { get; private set; }

        public void RaiseChangeStateEvent(Model.HurtStates newState) {
            if (!photonView.isActiveAndEnabled) return;

            ChangeState(newState);
            photonView.RPC("ChangeState", RpcTarget.Others, newState);
        }

        public void ChangeState(Model.HurtStates newState) {
            State.Exit();
            State = StateFactory.HurtFSFromEnum(newState, this);
            State.Enter();
        }
        
        private void Update() => State.Update();

        private void FixedUpdate() => State.FixedUpdate();

        private void LateUpdate() => State.LateUpdate();
    }
}