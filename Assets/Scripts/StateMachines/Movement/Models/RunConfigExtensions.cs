using UnityEngine;

namespace StateMachines.Movement.Models {
    public static class RunConfigExtensions {
        public static RunConfig CreateInstance(this RunConfig config) {
            var instance = ScriptableObject.CreateInstance<RunConfig>();
            instance.dashDuration = config.dashDuration;
            instance.dashVelocity = config.dashVelocity;
            instance.maxVelocity = config.maxVelocity;
            instance.runVelocity = config.runVelocity;
            return instance;
        }
    }
}