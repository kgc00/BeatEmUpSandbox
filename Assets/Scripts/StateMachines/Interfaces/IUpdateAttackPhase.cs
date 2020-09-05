using StateMachines.Attacks.Models;

namespace StateMachines.Interfaces {
    public interface IUpdateAttackPhase {
        AttackPhase Phase { get; }
        void EnterStartup();
        void EnterActive();
        void EnterRecovery();
    }
}