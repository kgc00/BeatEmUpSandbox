using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using StateMachines.Movement.Models;
using UnityEngine;

namespace StateMachines.Network {
    public static class SetMovementDirEvent {
        public static void SendSetMovementDirEvent(float moveDir, Vector3 localScale, int id) =>
            PhotonNetwork.RaiseEvent(NetworkedEventCodes.SetMovementDirEventCode,
                new object[] {moveDir, localScale, id},
                new RaiseEventOptions {Receivers = ReceiverGroup.All},
                SendOptions.SendReliable);
    }
}