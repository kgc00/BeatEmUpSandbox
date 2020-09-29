using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using StateMachines.Movement.Models;

namespace StateMachines.Network {
    public static class ChangeRunStateEvent {

        public static void SendChangeRunStateEvent(RunStates newState) {
            // Array contains the target position and the IDs of the selected units
            var content = new object[] {newState};
            // You would have to set the Receivers to All in order to receive this event on the local client as well
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
        
            PhotonNetwork.RaiseEvent(NetworkedEventCodes.ChangeRunStateEventCode, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }
}