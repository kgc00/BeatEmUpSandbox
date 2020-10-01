using Photon.Pun;
using UnityEngine;

namespace General {
    public class DontDestroyPun : MonoBehaviourPunCallbacks, IPunObservable {
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;
        private void Start() {
            if (photonView.IsMine) {
                LocalPlayerInstance = gameObject;
            }

            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(gameObject);
            }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            // throw new System.NotImplementedException();
        }
    }
}