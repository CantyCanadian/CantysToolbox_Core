using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

namespace Canty.StateMachineSystem
{
    public class StateMachineObject<StateType, StateEnum, ContextType> : StateMachineObjectBase<StateType, StateEnum, ContextType>
        where StateType : StateBase<StateEnum, ContextType>
        where StateEnum : Enum
        where ContextType : StateMachineContextBase, new()
    {
        protected override void GenerateStateList(ref Dictionary<StateEnum, StateType> list)
        {
            Type stateType = typeof(StateType);
            list = Assembly.GetAssembly(stateType).GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(stateType))
                .Select(type => (StateType)Activator.CreateInstance(type))
                .ToDictionary(instance => instance.GetStateEnum(), instance => instance);
        }

        public StateMachineObject(StateEnum defaultState, GameObject owner)
            : base(defaultState, owner)
        { }
    }

    public class MonoStateMachineObject<StateType, StateEnum, ContextType> : StateMachineObjectBase<StateType, StateEnum, ContextType>
        where StateType : MonoStateBase<StateEnum, ContextType>
        where StateEnum : Enum
        where ContextType : StateMachineContextBase, new()
    {
        protected override void GenerateStateList(ref Dictionary<StateEnum, StateType> list)
        {
            list = _owner.GetComponentsInChildren<StateType>()
                .ToDictionary(obj => obj.GetStateEnum(), obj => obj);
        }

        public MonoStateMachineObject(StateEnum defaultState, GameObject owner)
            : base(defaultState, owner)
        { }
    }

    public abstract class StateMachineObjectBase<StateType, StateEnum, ContextType>
        where StateType : IState<StateEnum, ContextType>
        where StateEnum : Enum
        where ContextType : StateMachineContextBase, new()
    {
        public StateEnum CurrentState { get; private set; } = default;
        public StateType CurrentStateObject { get; private set; } = default;
        public List<StateType> States { get { return _stateObjects.Values.ToList(); } }

        protected Dictionary<StateEnum, StateType> _stateObjects = new Dictionary<StateEnum, StateType>();
        protected ContextType _context = new ContextType();

        protected GameObject _owner = null;

        protected Action<StateEnum, StateEnum> _onStateChanged = null;

        protected abstract void GenerateStateList(ref Dictionary<StateEnum, StateType> list);

        protected virtual void UpdateContext(ref ContextType context) { }

        public void RegisterOnStateChangedCallback(Action<StateEnum, StateEnum> callback)
        {
            if (_onStateChanged == null)
            {
                _onStateChanged = new Action<StateEnum, StateEnum>(callback);
            }
            else
            {
                _onStateChanged += callback;
            }
        }

        public void ChangeState(StateEnum newState, bool force = false)
        {
            if (!force && CurrentState.Equals(newState))
                return;

            CurrentStateObject?.OnStateExit();

            StateEnum oldState = CurrentState;
            CurrentState = newState;
            CurrentStateObject = _stateObjects[newState];

            CurrentStateObject?.OnStateEnter(oldState);

            _onStateChanged?.Invoke(oldState, newState);
        }

        public void ChangeStateWithoutExit(StateEnum newState, bool force = false)
        {
            if (!force && CurrentState.Equals(newState))
                return;

            StateEnum oldState = CurrentState;
            CurrentState = newState;
            CurrentStateObject = _stateObjects[newState];

            CurrentStateObject?.OnStateEnter(oldState);

            _onStateChanged?.Invoke(oldState, newState);
        }

        public void ChangeStateWithoutEnter(StateEnum newState, bool force = false)
        {
            if (!force && CurrentState.Equals(newState))
                return;

            CurrentStateObject?.OnStateExit();

            StateEnum oldState = CurrentState;
            CurrentState = newState;
            CurrentStateObject = _stateObjects[newState];

            _onStateChanged?.Invoke(oldState, newState);
        }

        public void UpdateState()
        {
            UpdateContext(ref _context);
            CurrentStateObject.OnStateUpdate(_context);
        }

        public StateMachineObjectBase(StateEnum defaultState, GameObject owner)
        {
            _owner = owner;
            GenerateStateList(ref _stateObjects);
            ChangeState(defaultState, true);
        }
    }
}