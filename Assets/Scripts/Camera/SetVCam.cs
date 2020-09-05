using Cinemachine;
using UnityEngine;

public class SetVCam : MonoBehaviour {
    void Awake() {
        var vcam = FindObjectOfType<CinemachineVirtualCamera>();
        if (!vcam) return;
        vcam.Follow = gameObject.transform;
    }
}