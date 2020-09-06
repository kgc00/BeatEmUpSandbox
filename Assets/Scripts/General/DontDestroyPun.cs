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
            
            
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }
        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        }
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            // throw new System.NotImplementedException();
        }
        
        public override void OnDisable()
        {
            // Always call the base to remove callbacks
            base.OnDisable ();

            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;

        }
        
        /// <summary>
        /// MonoBehaviour method called after a new level of index 'level' was loaded.
        /// We recreate the Player UI because it was destroy when we switched level.
        /// Also reposition the player if outside the current arena.
        /// </summary>
        /// <param name="level">Level index loaded</param>
        void CalledOnLevelWasLoaded(int level)
        {
            // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
            // if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            // {
            //     transform.position = new Vector3(-4f, -2f, 0f);
            // }

            // GameObject _uiGo = Instantiate(this.playerUiPrefab);
            // _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }
    }
}