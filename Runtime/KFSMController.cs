using KuzuStudios.KBlackboard;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

namespace KuzuStudios.KFSM
{
    [CreateAssetMenu(fileName = "new KFSM Contoller", menuName = "KuzuStudios/KFSM/Controller")]
    public class KFSMController : ScriptableObject
    {
        public List<SKFSMStateContainer> StateContainer = new();
        public KBlackboardController Blackboard;

        public KFSMState CurrentState;
        public KFSMState StartingState;

        private readonly Dictionary<string, KFSMState> _stateLookup = new();

        #region Callback Functions
        public void OnInitialize()
        {
            _stateLookup.Clear();
            foreach (var state in _stateLookup.Values)
            {
                state.OnInitialize(this);
            }
        }

        public void OnStart() 
        {
            StartState();
        }

        public void OnUpdate() { if (CurrentState != null) CurrentState.OnUpdate(); }
        public void OnFixedUpdate() { if (CurrentState != null) CurrentState.OnFixedUpdate(); }

        public KFSMController Clone()
        {
            var clone = ScriptableObject.Instantiate(this);

            var oldStateContainers = clone.StateContainer.ToList();
            clone.StateContainer.Clear();
            clone._stateLookup.Clear();
            foreach (var sc in oldStateContainers)
            {
                var stateClone = sc.State.Clone();
                if (sc.State == StartingState) clone.StartingState = stateClone;
                clone.StateContainer.Add(new SKFSMStateContainer { DisplayName = sc.DisplayName, State = stateClone });
                clone._stateLookup[stateClone.DisplayName] = stateClone;
                clone._stateLookup[stateClone.StateName] = stateClone;
            }

            return clone;
        }
        #endregion

        #region Animation Functions
        public void OnAnimationTrigger(int index) { if (CurrentState != null) CurrentState.OnAnimationTrigger(index); }
        public void OnAnimationFinishTrigger() { if (CurrentState != null) CurrentState.OnAnimationFinishTrigger(); }
        #endregion

        #region State Functions
        public void StartState()
        {
            if (StartingState == null) Debug.LogWarning($"[FSM Controller] Starting State is null. Please set a starting state in the controller.");
            else if (!_stateLookup.Values.Contains(StartingState)) Debug.LogWarning($"[FSM Controller] Starting State is not in the list of states. Please add the starting state to the controller.");
            else
            {
                CurrentState = StartingState;
                CurrentState.OnEnter();
            }
        }

        public void SetStartingState(KFSMState state)
        {
            if (state == null) Debug.LogWarning($"[FSM Controller] State is null. Please set a valid state as the starting state.");
            else if (!_stateLookup.Values.Contains(state)) Debug.LogWarning($"[FSM Controller] State {state.name} is not in the list of states. Please add the state to the controller.");
            else StartingState = state;
        }

        public void SetStartingState(string stateName)
        {
            var state = _stateLookup[stateName];
            SetStartingState(state);
        }

        public void ChangeState(KFSMState newState)
        {
            if (newState == null) Debug.LogWarning($"[FSM Controller] New State is null. Please set a valid state to change to.");
            else if (!_stateLookup.Values.Contains(newState)) Debug.LogWarning($"[FSM Controller] New State {newState.name} is not in the list of states. Please add the state to the controller.");
            else
            {
                if (CurrentState != null) CurrentState.OnExit();
                CurrentState = newState;
                CurrentState.OnEnter();
            }
        }

        public void ChangeState(string stateName)
        {
            var state = _stateLookup[stateName];
            ChangeState(state);
        }

        public KFSMState GetState(string stateName)
        {
            var state = _stateLookup[stateName];

            if (state == null) state = _stateLookup[stateName];

            if (state == null) Debug.LogWarning($"[FSM Controller] State {stateName} is not in the list of states. Please add the state to the controller.");
            return state;
        }

        public bool TryGetState(string stateName, out KFSMState state)
        {
            state = GetState(stateName);

            return state != null;
        }

        public List<SKFSMStateContainer> GetStateContainers()
        {
            List<SKFSMStateContainer> stateContainers = new();
            foreach (var state in StateContainer)
            {
                SKFSMStateContainer tmpContainer = new()
                {
                    DisplayName = state.DisplayName,
                    State = state.State
                };
                stateContainers.Add(tmpContainer);
            }

            return stateContainers;
        }

        public void AddState(SKFSMStateContainer container)
        {
            if (!StateContainer.Contains(container))
            {
                StateContainer.Add(container);
            }
        }

        public void SetState(SKFSMStateContainer container, SKFSMStateContainer newContainer)
        {
            if (StateContainer.Contains(container))
            {
                var index = StateContainer.IndexOf(container);
                StateContainer[index] = newContainer;
            }
        }

        public void RemoveState(SKFSMStateContainer container)
        {
            if (StateContainer.Contains(container))
            {
                StateContainer.Remove(container);
            }
        }

        public void SetDisplayName(int index, SKFSMStateContainer updatedContainer)
        {
            StateContainer[index] = updatedContainer;
        }

        public void ReorderStates(int oldIndex, int newIndex)
        {
            if (oldIndex < 0 || newIndex < 0 || oldIndex >= StateContainer.Count || newIndex >= StateContainer.Count)
            {
                Debug.LogWarning($"[KFSM Controller] Invalid state index change from {oldIndex} to {newIndex}. Index out of range.");
                return;
            }

            var oldIndexParameter = StateContainer[oldIndex];

            StateContainer.RemoveAt(oldIndex);
            StateContainer.Insert(newIndex, oldIndexParameter);
        }
        #endregion
    }
}