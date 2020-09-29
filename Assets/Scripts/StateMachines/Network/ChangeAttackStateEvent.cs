using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace StateMachines.Network {
    public static class ChangeAttackStateEvent {
        public static void SendChangeAttackStateEvent(AttackStates newState) {
            // Array contains the target position and the IDs of the selected units
            var content = newState;
            // You would have to set the Receivers to All in order to receive this event on the local client as well
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
        
            PhotonNetwork.RaiseEvent(NetworkedEventCodes.ChangeAttackStateEventCode, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }
}