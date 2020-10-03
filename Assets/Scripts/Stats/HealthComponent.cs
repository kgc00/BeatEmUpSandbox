using System;
using UnityEngine;

namespace Stats {
    public class HealthComponent : MonoBehaviour {
        [SerializeField] private UnitStats stats;
        public float MaxHp { get; private set; }
        public float CurrentHp { get; private set; }
        public bool Invulnerable { get; private set; }
        public bool IsDead => CurrentHp <= 0;
        public void SetInvulnerable() => Invulnerable = true;
        public void SetVulnerable() => Invulnerable = false;

        private void OnEnable() {
            MaxHp = stats.maxHealth;
            CurrentHp = MaxHp;
        }

        public void Damage(float amount) {
            Debug.Log($"Adjusting {gameObject.name}'s current health by {-Math.Abs(amount)}.");

            if (Invulnerable) return;
            AdjustHealth(-Math.Abs(amount));
        }

        public void Heal(float amount) {
            Debug.Log($"Adjusting {gameObject.name}'s current health by {Math.Abs(amount)}.");

            AdjustHealth(Math.Abs(amount));
        }

        /// <summary>
        /// Adjust unit's health:
        /// negative values => damage
        /// positive values => heal
        /// </summary>
        /// <param name="amount"></param>
        private void AdjustHealth(float amount) {
            var prevAmount = CurrentHp;
            var newAmount = Mathf.Clamp(CurrentHp + amount, 0, MaxHp);

            // Debug.Log($"Adjusting {Owner.name}'s current health from {prevAmount} to {newAmount}.");

            CurrentHp = newAmount;

            if (CurrentHp <= 0) {
                Debug.Log($"{gameObject.name} died");
                Destroy(gameObject);
            }
        }

        internal void Refill() {
            var prevAmount = CurrentHp;
            CurrentHp = MaxHp;
        }
    }
}