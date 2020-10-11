using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using StateMachines.Movement.Models;
using StateMachines.State;

namespace StateMachines.Network {
    public static class SetMovementValuesEvent {

        public static void SendSetMovementValuesEvent(UnitMovementData newMovementData) {
            // Array contains the target position and the IDs of the selected units
            var content = new object[] {
                newMovementData.moveDir,
                newMovementData.jumpsLeft,
                newMovementData.dashesLeft,
                newMovementData.dashTimeLapsed
            };
            // You would have to set the Receivers to All in order to receive this event on the local client as well
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
        
            PhotonNetwork.RaiseEvent(NetworkedEventCodes.SetMovementValuesEventCode, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }
}