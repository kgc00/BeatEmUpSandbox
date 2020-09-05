using StateMachines.Attacks.Models;

namespace StateMachines.Interfaces {
    public interface IExposeAttackPhase {
        
        AttackPhase CurrentPhase();
    }
}