using System;
using UnityEngine;

namespace Canty.StateMachineSystem
{
    public abstract class StateBase<StateEnum, ContextType> : IState<StateEnum, ContextType>
        where StateEnum : Enum
        where ContextType : StateMachineContextBase, new()
    {
        protected StateMachineObject<StateBase<StateEnum, ContextType>, StateEnum, ContextType> _owner = null;

        public void SetOwner(StateMachineObject<StateBase<StateEnum, ContextType>, StateEnum, ContextType> owner)
        {
            _owner = owner;
        }

        public abstract StateEnum GetStateEnum();

        public virtual void OnStateEnter(StateEnum lastState, ContextType context) { }

        /// <summary>
        /// Update loop that only happens when this state is the current state.
        /// </summary>
        public virtual void OnStateUpdate(ContextType context) { }

        public virtual void OnStateExit(ContextType context) { }
    }

    public abstract class MonoStateBase<StateEnum, ContextType> : MonoBehaviour, IState<StateEnum, ContextType>
        where StateEnum : Enum
        where ContextType : StateMachineContextBase, new()
    {
        protected StateMachineObjectBase<StateEnum> _owner = null;

        public void SetOwner(StateMachineObjectBase<StateEnum> owner)
        {
            _owner = owner;
        }

        public abstract StateEnum GetStateEnum();

        public virtual void OnStateEnter(StateEnum lastState, ContextType context) { }

        /// <summary>
        /// Update loop that only happens when this state is the current state.
        /// </summary>
        public virtual void OnStateUpdate(ContextType context) { }

        public virtual void OnStateExit(ContextType context) { }
    }

    public interface IState<StateEnum, ContextType>
        where StateEnum : Enum
        where ContextType : StateMachineContextBase, new()
    {
        StateEnum GetStateEnum();

        void OnStateEnter(StateEnum lastState, ContextType context);

        /// <summary>
        /// Update loop that only happens when this state is the current state.
        /// </summary>
        void OnStateUpdate(ContextType context);

        void OnStateExit(ContextType context);
    }
}