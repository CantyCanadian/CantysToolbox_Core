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

            foreach (StateType type in list.Values)
                type.SetOwner(this as StateMachineObject<StateBase<StateEnum, ContextType>, StateEnum, ContextType>);
        }
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

            foreach(StateType type in list.Values)
                type.SetOwner(this);
        }
    }

    public abstract class StateMachineObjectBase<StateType, StateEnum, ContextType> : StateMachineObjectBase<StateEnum>
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

        protected abstract void GenerateStateList(ref Dictionary<StateEnum, StateType> list);

        protected virtual void UpdateContext(ref ContextType context) { }

        public override void ChangeState(StateEnum newState, bool force = false)
        {
            if (!force && CurrentState.Equals(newState))
                return;

            UpdateContext(ref _context);

            CurrentStateObject?.OnStateExit(_context);

            StateEnum oldState = CurrentState;
            CurrentState = newState;
            CurrentStateObject = _stateObjects[newState];

            CurrentStateObject?.OnStateEnter(oldState, _context);

            _onStateChanged?.Invoke(oldState, newState);
        }

        public override void ChangeStateWithoutExit(StateEnum newState, bool force = false)
        {
            if (!force && CurrentState.Equals(newState))
                return;

            UpdateContext(ref _context);

            StateEnum oldState = CurrentState;
            CurrentState = newState;
            CurrentStateObject = _stateObjects[newState];

            CurrentStateObject?.OnStateEnter(oldState, _context);

            _onStateChanged?.Invoke(oldState, newState);
        }

        public override void ChangeStateWithoutEnter(StateEnum newState, bool force = false)
        {
            if (!force && CurrentState.Equals(newState))
                return;

            UpdateContext(ref _context);

            CurrentStateObject?.OnStateExit(_context);

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

        public virtual void Initialize(StateEnum defaultState, GameObject owner)
        {
            _owner = owner;
            GenerateStateList(ref _stateObjects);
            ChangeState(defaultState, true);
        }
    }

    public abstract class StateMachineObjectBase<StateEnum>
       where StateEnum : Enum
    {
        protected Action<StateEnum, StateEnum> _onStateChanged = null;

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

        public abstract void ChangeState(StateEnum newState, bool force = false);

        public abstract void ChangeStateWithoutExit(StateEnum newState, bool force = false);

        public abstract void ChangeStateWithoutEnter(StateEnum newState, bool force = false);
    }
}