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
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace TestSandbox.PlayMode {
    public class KeyLoggerTests {
        [TestFixture]
        class ArenaTestFixture : InputTestFixture {
            private Keyboard keyboard;
            private Mouse mouse;
            private PlayerInput player;

            private string inputSettingsPath =
                "C:\\Users\\kirby\\Documents\\Game Dev\\2020\\BeatEmUpSandbox\\Assets\\Scripts\\Input\\PlayerActions.inputactions";

            [SetUp] // if asynchronous code is necessary use [UnitySetUp]
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

            [UnityTearDown]
            public IEnumerator Disconnect() {
                Debug.Log("tearing down");
                if (!PhotonNetwork.IsConnected) yield break;

                PhotonNetwork.Disconnect();
                while (!PhotonNetwork.IsConnected) {
                    yield return new WaitForSeconds(0.25f);
                    if (!PhotonNetwork.IsConnected) yield break;
                }
                
                PhotonNetwork.LoadLevel(NetworkConfig.launcherLevelName);
                yield return new WaitForSeconds(0.5f);
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
            public IEnumerator InputLoggerReceivesInputEvents() {
                yield return SetupRemotePlayer();

                var action1 = player.actions["Jump"];
                var action2 = player.actions["Move"];

                var logger = player.GetComponent<InputLogger>();

                using (var trace = new InputActionTrace()) {
                    // must subscribe BEFORE event occurs
                    trace.SubscribeTo(action1);
                    trace.SubscribeTo(action2);

                    PressAndRelease(keyboard.spaceKey);
                    InputSystem.Update(); // 0.03f - 1 frame
                    PressAndRelease(keyboard.aKey);
                    InputSystem.Update();

                    Assert.That(logger.Actions.Count, Is.EqualTo(4)); // actions * 2
                }
            }

            [UnityTest]
            public IEnumerator InputLoggerRemovesOldEvents() {
                yield return SetupRemotePlayer();

                var action1 = player.actions["Jump"];
                var action2 = player.actions["Move"];

                var logger = player.GetComponent<InputLogger>();

                using (var trace = new InputActionTrace()) {
                    {
                        // must subscribe BEFORE event occurs
                        trace.SubscribeTo(action1);
                        trace.SubscribeTo(action2);

                        PressAndRelease(keyboard.spaceKey);
                        // run next event halfway through our timeout
                        yield return new WaitForSeconds(InputLogger.EventTimeDeletionThreshold / 2);
                        
                        PressAndRelease(keyboard.aKey);
                        
                        // without waiting for two frames the inputs just barely stay valid
                        // need the extra frames for them to time out and be deleted
                        yield return new WaitForEndOfFrame();
                        yield return new WaitForEndOfFrame();

                        Assert.That(logger.Actions.Count, Is.EqualTo(4));

                        yield return new WaitForSeconds(InputLogger.EventTimeDeletionThreshold / 2);
                        Assert.That(logger.Actions.Count, Is.EqualTo(2)); 
                        
                        yield return new WaitForSeconds(InputLogger.EventTimeDeletionThreshold / 2);
                        Assert.That(logger.Actions.Count, Is.EqualTo(0));
                    }
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