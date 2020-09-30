using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using StateMachines.Movement.Models;

namespace StateMachines.Network {
    public static class ChangeRunStateEvent {
        public static void SendChangeRunStateEvent(RunStates newState, int id) =>
            PhotonNetwork.RaiseEvent(NetworkedEventCodes.ChangeRunStateEventCode,
                new object[] {newState, id},
                new RaiseEventOptions {Receivers = ReceiverGroup.All},
                SendOptions.SendReliable);
    }
}