using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using StateMachines.Movement.Models;

namespace StateMachines.Network {
    public static class SetMovementValuesEvent {

        public static void SendSetMovementValuesEvent(MovementValues newValues) {
            // Array contains the target position and the IDs of the selected units
            var content = new object[] {
                newValues.moveDir,
                newValues.jumpsLeft,
                newValues.dashesLeft,
                newValues.dashTimeLapsed
            };
            // You would have to set the Receivers to All in order to receive this event on the local client as well
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
        
            PhotonNetwork.RaiseEvent(NetworkedEventCodes.SetMovementValuesEventCode, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }
}