﻿using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using StateMachines.Movement.Models;

namespace StateMachines.Network {
    public static class ChangeJumpStateEvent {

        public static void SendChangeJumpStateEvent(JumpStates newState) {
            // Array contains the target position and the IDs of the selected units
            var content = new object[] {newState};
            // You would have to set the Receivers to All in order to receive this event on the local client as well
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
        
            PhotonNetwork.RaiseEvent(NetworkedEventCodes.ChangeJumpStateEventCode, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }
}