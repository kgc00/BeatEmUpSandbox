using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using StateMachines.Movement.Models;
using UnityEngine;

namespace StateMachines.Network {
    public static class RunCollisionEvent {
        // If you have multiple custom events, it is recommended to define them in the used class
        public const byte RunCollisionEventCode = 4;

        public static void SendRunCollisionEvent(Collision2D other) {
            var content = other;
            
            // You would have to set the Receivers to All in order to receive this event on the local client as well
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.Others};
        
            PhotonNetwork.RaiseEvent(RunCollisionEventCode, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }
}