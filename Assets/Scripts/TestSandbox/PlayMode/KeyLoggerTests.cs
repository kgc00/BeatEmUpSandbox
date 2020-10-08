using System.Collections;
using System.IO;
using System.Linq;
using NUnit.Framework;
using StateMachines.Actions;
using StateMachines.Logger;
using StateMachines.Messages;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.TestTools;

namespace TestSandbox.PlayMode {
    public class KeyLoggerTests {
        [TestFixture]
        class ArenaTestFixture : InputTestFixture {
            [UnityTest]
            public IEnumerator DoesTrackInputEvents() {
                Keyboard keyboard = InputSystem.AddDevice<Keyboard>();
                Mouse mouse = InputSystem.AddDevice<Mouse>();

                var prefab = new GameObject();
                prefab.SetActive(false);
                var prefabPlayerInput = prefab.AddComponent<PlayerInput>();
                var kActions = File.ReadAllText("C:\\Users\\kirby\\Documents\\Game Dev\\2020\\BeatEmUpSandbox\\Assets\\Scripts\\Input\\PlayerActions.inputactions");
                prefabPlayerInput.actions = InputActionAsset.FromJson(kActions);

                var player = PlayerInput.Instantiate(prefab, controlScheme: "Keyboard&Mouse");
                
                foreach (var a in player.actions) {
                    Debug.Log(a.name);
                }
                
                var action1 = player.actions["Jump"];
                var action2 = player.actions["Move"];
                action1.Enable();
                action2.Enable();
                
                using (var trace = new InputActionTrace()) {
                    trace.SubscribeTo(action1);
                    trace.SubscribeTo(action2);
                    
                    PressAndRelease(keyboard.spaceKey);
                    PressAndRelease(keyboard.aKey);
                    PressAndRelease(keyboard.dKey);
                    InputSystem.Update();
                    currentTime = 0.234f;

                    var actions = trace.ToArray();
                    foreach (var a in actions) {
                        Debug.Log(a);
                    }

                    Assert.That(actions.Length, Is.EqualTo(1));
                }

                yield break;
            }
        }

        class SomeTestFixture : InputTestFixture {
            [Test]
            public void Actions_WhenDisabled_CancelAllStartedInteractions() {
                var gamepad = InputSystem.AddDevice<Gamepad>();

                var action1 = new InputAction("action1", binding: "<Gamepad>/buttonSouth", interactions: "Hold");
                var action2 = new InputAction("action2", binding: "<Gamepad>/leftStick");

                action1.Enable();
                action2.Enable();

                Press(gamepad.buttonSouth);
                Set(gamepad.leftStick, new Vector2(0.123f, 0.234f));

                using (var trace = new InputActionTrace()) {
                    trace.SubscribeTo(action1);
                    trace.SubscribeTo(action2);

                    currentTime += 0.234f;
                    action1.Disable();
                    action2.Disable();

                    var actions = trace.ToArray();
                    foreach (var a in actions) {
                        Debug.Log(a);
                    }

                    Assert.That(actions.Length, Is.EqualTo(2));
                }
            }
        }

        [Test]
        public void PlayerContainsInputLogger() {
            var fixture = new ArenaTestFixture();

            var go = Resources.Load<GameObject>("Player");
            var logger = go.GetComponent<InputLogger>();
            Assert.That(logger != null);
        }
    }
}