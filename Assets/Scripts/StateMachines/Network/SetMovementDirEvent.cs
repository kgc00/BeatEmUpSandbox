using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using StateMachines.Movement.Models;

namespace StateMachines.Network {
    public static class SetMovementDirEvent {
        public static void SendSetMovementDirEvent(float moveDir) {
            // Array contains the target position and the IDs of the selected units
            var content = moveDir;
            // You would have to set the Receivers to All in order to receive this event on the local client as well
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.Others};

            PhotonNetwork.RaiseEvent(NetworkedEventCodes.SetMovementDirEventCode, content, raiseEventOptions,
                SendOptions.SendReliable);
        }
    }
}