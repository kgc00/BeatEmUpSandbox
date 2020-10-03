﻿using System;
using Photon.Pun;
using StateMachines.Movement;
using StateMachines.Movement.Models;
using UnityEngine;

namespace StateMachines.Triggers {
    public class TriggerGround : MonoBehaviour {
        [SerializeField] PhotonView photonView;

        public void OnEnable() {
            photonView = GetComponentInParent<MovementFSM>().photonView
                ? GetComponentInParent<MovementFSM>().photonView
                : throw new NullReferenceException("photonView");
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.gameObject.CompareTag("Board") || !photonView.IsMine) return;

            photonView.RPC("SetTouchingGround", RpcTarget.All, true);
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (!other.gameObject.CompareTag("Board") || !photonView.IsMine) return;
            
            photonView.RPC("SetTouchingGround", RpcTarget.All, false);
        }
    }
}