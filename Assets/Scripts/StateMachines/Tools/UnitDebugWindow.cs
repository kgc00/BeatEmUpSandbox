using System;
using System.Linq;
using General;
using Photon.Pun;
using StateMachines.Attacks;
using StateMachines.Movement;
using StateMachines.State;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StateMachines.Tools {
    public class UnitDebugWindow : EditorWindow {
        private GameObject unit;
        private MovementFSM movementFSM;
        private AttackFSM attackFSM;
        private UnitDataStore movementDataStore;

        [MenuItem("Tools/Unit Debugger")]
        public static void ShowWindow() => GetWindow<UnitDebugWindow>();

        //
        // private void Update() {
        //     if (unit == null && Editor.)
        //     unit = EditorGUILayout.ObjectField(unit, typeof(GameObject), true) as GameObject;
        // }

        private void OnGUI() {
            if (!EditorApplication.isPlaying) {
                EditorGUILayout.TextArea("Enter Play Mode for Info...");
                return;
            }

            EditorGUILayout.LabelField("Unit", EditorStyles.boldLabel);
            unit = FindObjectsOfType<PhotonView>()?.Where(x => x.IsMine).Select(x => x.gameObject).FirstOrDefault();

            if (unit == null) return;


            Repaint();
            if (movementFSM == null) movementFSM = unit.GetComponent<MovementFSM>();
            if (attackFSM == null) attackFSM = unit.GetComponent<AttackFSM>();
            if (movementDataStore == null) movementDataStore = unit.GetComponent<UnitDataStore>();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("State:", EditorStyles.boldLabel);
            EditorGUILayout.TextArea($"Run: {Helpers.GetUniqueStateName(movementFSM.Run.State.ToString())}");
            EditorGUILayout.TextArea($"Jump: {Helpers.GetUniqueStateName(movementFSM.Jump.State.ToString())}");
            EditorGUILayout.TextArea($"Attack: {Helpers.GetUniqueStateName(attackFSM.State.ToString())}");

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Unit Movement Values:", EditorStyles.boldLabel);
            
            GUILayout.Box("moveDir: " + movementDataStore.store.moveDir);
            GUILayout.Box("jumps left: " + movementDataStore.store.jumpsLeft);
            GUILayout.Box("air dashes left: " + movementDataStore.store.dashesLeft);
            GUILayout.Box("touching wall: " + movementDataStore.store.touchingWall);
            GUILayout.Box("touching ground: " + movementDataStore.store.touchingGround);
            GUILayout.Box("minJumpDuration: " + movementFSM.Jump.Config.minJumpDuration);
        }
    }
}