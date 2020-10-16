using Photon.Pun;
using StateMachines.HurtStates.States;
using StateMachines.Interfaces;
using StateMachines.Network;

namespace StateMachines.HurtStates {
    public class HurtFSM : MonoBehaviourPun, IChangeStatePun<Model.HurtStates> {
        public HurtFS State { get; private set; }
        public void RaiseChangeStateEvent(Model.HurtStates newState) {
            if (photonView.isActiveAndEnabled)
                photonView.RPC("ChangeState", RpcTarget.All, newState);
        }

        public void ChangeState(Model.HurtStates newState) {
            State.Exit();
            State = StateFactory.HurtFSFromEnum(newState, this);
            State.Enter();
        }
    }
}