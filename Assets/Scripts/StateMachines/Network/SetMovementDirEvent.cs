using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using StateMachines.Movement.Models;

namespace StateMachines.Network {
    public static class SetMovementDirEvent {
        public static void SendSetMovementDirEvent(float moveDir, int id) =>
            PhotonNetwork.RaiseEvent(NetworkedEventCodes.SetMovementDirEventCode,
                new object[] {moveDir, id},
                new RaiseEventOptions {Receivers = ReceiverGroup.All},
                SendOptions.SendReliable);
    }
}