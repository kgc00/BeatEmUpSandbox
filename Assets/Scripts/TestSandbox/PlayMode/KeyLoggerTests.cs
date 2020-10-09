using System;
using System.Collections;
using System.IO;
using System.Linq;
using General;
using NUnit.Framework;
using Photon.Pun;
using StateMachines.Actions;
using StateMachines.Logger;
using StateMachines.Messages;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace TestSandbox.PlayMode {
    public class KeyLoggerTests {
        
        [TestFixture]
        class ArenaTestFixture : InputTestFixture {
            bool isConnecting;
            private Keyboard keyboard;
            private Mouse mouse;
            private PlayerInput player;
            private string inputSettingsPath =
                "C:\\Users\\kirby\\Documents\\Game Dev\\2020\\BeatEmUpSandbox\\Assets\\Scripts\\Input\\PlayerActions.inputactions";

            [SetUp]
            public void SetupInput() {
                keyboard = InputSystem.AddDevice<Keyboard>();
                mouse = InputSystem.AddDevice<Mouse>();
            }

            private IEnumerator SetupRemotePlayer() {
                PhotonNetwork.OfflineMode = true;
                new GameObject("launch").AddComponent<AutoLaunch>();
                yield return new WaitUntil(() => PhotonNetwork.InRoom);
                yield return new WaitForSeconds(.5f); // grace period to allow objects to load in
                player = Object.FindObjectOfType<PlayerInput>() ?? throw new Exception("Unable to assign player");
            }

            private void SetupLocalPlayer() {
                var prefab = new GameObject();
                prefab.SetActive(false);
                var prefabPlayerInput = prefab.AddComponent<PlayerInput>();
                var kActions = File.ReadAllText(inputSettingsPath);
                prefabPlayerInput.actions = InputActionAsset.FromJson(kActions);
                player = PlayerInput.Instantiate(prefab, controlScheme: "Keyboard&Mouse");
                player.actions.Enable();
            }

            [UnityTest]
            public IEnumerator DoesTrackInputEvents() {
                SetupLocalPlayer();
                var action1 = player.actions["Jump"];
                var action2 = player.actions["Move"];

                using (var trace = new InputActionTrace()) {
                    // must subscribe BEFORE event occurs
                    trace.SubscribeTo(action1);
                    trace.SubscribeTo(action2);

                    PressAndRelease(keyboard.spaceKey);
                    InputSystem.Update(); // 0.03f - 1 frame
                    PressAndRelease(keyboard.aKey);

                    var actions = trace.ToArray();

                    Assert.That(actions.Length, Is.EqualTo(6)); // actions * 3
                }

                yield break;
            }

            [UnityTest]
            public IEnumerator CanAddInputLogger() {
                yield return SetupRemotePlayer();
                
                var action1 = player.actions["Jump"];
                var action2 = player.actions["Move"];

                var logger = player.GetComponent<InputLogger>();

                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();

                using (var trace = new InputActionTrace()) {
                    // must subscribe BEFORE event occurs
                    trace.SubscribeTo(action1);
                    trace.SubscribeTo(action2);

                    PressAndRelease(keyboard.spaceKey);
                    InputSystem.Update(); // 0.03f - 1 frame
                    yield return new WaitForEndOfFrame();
                    PressAndRelease(keyboard.aKey);
                    InputSystem.Update(); 
                    yield return new WaitForEndOfFrame();
                    
                    var actions = trace.ToArray();
                    LogActionsPerformed(actions);
                    yield return new WaitForSeconds(1);
                    
                    Assert.That(logger.Actions.Count, Is.EqualTo(4)); // actions * 2
                }
            }

            private static void LogActionsPerformed(InputActionTrace.ActionEventPtr[] actions) {
                foreach (var a in actions) {
                    Debug.Log(a);
                }
            }

            private static void SetInputSystemTime(InputTestFixture fixture) {
                var time = 0.234f;
                fixture.currentTime = time; // set "time" of input system. tracked in the event
            }

            private static void LogInputActions(PlayerInput player) {
                foreach (var a in player.actions) {
                    Debug.Log(a.name);
                }
            }
        }
    }
}