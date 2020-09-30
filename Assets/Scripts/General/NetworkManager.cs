using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace General {
    public class NetworkManager : MonoBehaviourPunCallbacks {
        public static NetworkManager Instance;
        private GameObject instance;
        private string launchSceneName = "launch";

        [Tooltip("The prefab to use for representing the player")] [SerializeField]
        private GameObject playerPrefab;

        void Start() {
            Instance = this;

            // in case we started this demo with the wrong scene being active, simply load the menu scene
            if (!PhotonNetwork.IsConnected) {
                SceneManager.LoadScene(launchSceneName);

                return;
            }

            if (playerPrefab == null) {
                // #Tip Never assume public properties of Components are filled up properly, always check and inform the developer of it.

                Debug.LogError(
                    "<Color=Red><b>Missing</b></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",
                    this);
            }
            else {
                if (DontDestroyPun.LocalPlayerInstance == null) {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

                    var xPos = 0;
                    if (PhotonNetwork.CurrentRoom.PlayerCount == 1) xPos = -4;
                    if (PhotonNetwork.CurrentRoom.PlayerCount == 2) xPos = 0;
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    PhotonNetwork.Instantiate(this.playerPrefab.name,
                        new Vector3(xPos,-2.6f,0f), Quaternion.identity, 0);
                }
                else {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }

        public override void OnPlayerEnteredRoom(Player other) {
            // Debug.Log("OnPlayerEnteredRoom() " + other.NickName); // not seen if you're the player connecting
            //
            // if (PhotonNetwork.IsMasterClient) {
            //     Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}",
            //         PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            //
            //     // LoadArena();
            // }
        }

        public override void OnPlayerLeftRoom(Player other) {
            // Debug.Log("OnPlayerLeftRoom() " + other.NickName); // seen when other disconnects
            //
            // if (PhotonNetwork.IsMasterClient) {
            //     Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}",
            //         PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            //
            //     // LoadArena();
            // }
        }

        public override void OnLeftRoom() {
            SceneManager.LoadScene(NetworkConfig.launcherLevelName);
        }


        public void LeaveRoom() {
            PhotonNetwork.LeaveRoom();
        }

        public void QuitApplication() {
            Application.Quit();
        }

        void LoadLobby() {
            if (!PhotonNetwork.IsMasterClient) {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }

            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);

            PhotonNetwork.LoadLevel(NetworkConfig.launcherLevelName);
        }

        void LoadArena() {
            if (!PhotonNetwork.IsMasterClient) {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }

            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);

            PhotonNetwork.LoadLevel(NetworkConfig.gameLevelName);
        }
    }
}