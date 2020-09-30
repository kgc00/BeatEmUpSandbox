using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace General {
    public class AutoLaunch : MonoBehaviourPunCallbacks {
    
        bool isConnecting;

        void Awake() {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start() {
            Connect();
        }
        
        public void Connect() {
            isConnecting = true;
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected) {
                print("Joining Room...");
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
            }
            else {
                print("Connecting...");

                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = NetworkConfig.gameVersion;
            }
        }

        public override void OnConnectedToMaster() {
            // we don't want to do anything if we are not attempting to join a room. 
            // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
            // we don't want to do anything.
            if (isConnecting) {
                print("OnConnectedToMaster: Next -> try to Join Random Room");
                print(
                    "PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");

                // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message) {
            print("<Color=Red>OnJoinRandomFailed</Color>: Next -> Create a new Room");
            // print(
            //     "PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            //
            // print($"Error codeL {returnCode}\n message: {message}");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = NetworkConfig.maxPlayers});
        }

        public override void OnDisconnected(DisconnectCause cause) {
            print("<Color=Red>OnDisconnected</Color> " + cause);
            print("PUN Basics Tutorial/Launcher:Disconnected");

            isConnecting = false;
        }

        public override void OnJoinedRoom() {
            print("<Color=Green>OnJoinedRoom</Color> with " + PhotonNetwork.CurrentRoom.PlayerCount + " Player(s)");
            print(
                "PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");

            PhotonNetwork.LoadLevel(NetworkConfig.gameLevelName);
        }
    }
}