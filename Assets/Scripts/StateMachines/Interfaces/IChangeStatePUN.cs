using System;

namespace StateMachines.Interfaces {
    public interface IChangeStatePun<in T> where T : Enum {
        void RaiseChangeStateEvent(T newState);
        void ChangeState(T newState);
    }
}