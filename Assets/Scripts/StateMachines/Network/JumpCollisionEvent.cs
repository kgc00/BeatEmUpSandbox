using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using StateMachines.Movement.Models;
using UnityEngine;

namespace StateMachines.Network {
    public static class JumpCollisionEvent {
        // If you have multiple custom events, it is recommended to define them in the used class
        public const byte JumpCollisionEventCode = 5;

        public static void SendJumpCollisionEvent(Collision2D other) {
            var content = other;
            
            // You would have to set the Receivers to All in order to receive this event on the local client as well
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
        
            PhotonNetwork.RaiseEvent(JumpCollisionEventCode, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }
}