using Cinemachine;
using Photon.Pun;
using UnityEngine;

namespace General {
    public class SetVCam : MonoBehaviourPun {
        void Awake() {
            if (!photonView.IsMine) return;
            var vcam = FindObjectOfType<CinemachineVirtualCamera>();
            if (!vcam) return;
            vcam.Follow = gameObject.transform;
        }
    }
}