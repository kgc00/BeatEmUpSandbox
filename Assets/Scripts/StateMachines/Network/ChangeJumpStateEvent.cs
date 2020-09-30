using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using StateMachines.Movement.Models;

namespace StateMachines.Network {
    public static class ChangeJumpStateEvent {
        public static void SendChangeJumpStateEvent(JumpStates newState, int id) =>
            PhotonNetwork.RaiseEvent(NetworkedEventCodes.ChangeJumpStateEventCode,
                new object[] {newState, id},
                new RaiseEventOptions {Receivers = ReceiverGroup.Others}, 
                SendOptions.SendReliable);
    }
}