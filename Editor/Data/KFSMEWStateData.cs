using Codice.Client.GameUI.Update;
using KuzuStudios.Kutils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

namespace KuzuStudios.KFSM.Editor
{
    public class KFSMEWStateData : ScriptableObject
    {
        public const string NEW_EMPTY_CONTAINER_DISPLAY_NAME = "Empty State";

        public KFSMEWData ParentData;

        public List<SKFSMStateContainer> States = new();
        public List<SKFSMStateContainer> StatesFiltered = new();
        public KFSMState SelectedState;

        public string SearchFieldText;

        private void OnEnable()
        {
            KFSMEditorWindow.OnControllerChange -= OnControllerChange;
            KFSMEditorWindow.OnControllerChange += OnControllerChange;
            KFSMEditorWindow.OnControllerChanged -= OnControllerChanged;
            KFSMEditorWindow.OnControllerChanged += OnControllerChanged;
            KFSMEditorWindow.OnControllerChangedFinal -= OnControllerChangedFinal;
            KFSMEditorWindow.OnControllerChangedFinal += OnControllerChangedFinal;

            KFSMEditorWindow.OnSelectedStateChange -= OnSelectedStateChange;
            KFSMEditorWindow.OnSelectedStateChange += OnSelectedStateChange;

            KFSMEditorWindow.OnSelectedStateDeselect -= OnSelectedStateDeselect;
            KFSMEditorWindow.OnSelectedStateDeselect += OnSelectedStateDeselect;

            KFSMEditorWindow.OnSearchFieldValueChange -= OnSearchFieldValueChange;
            KFSMEditorWindow.OnSearchFieldValueChange += OnSearchFieldValueChange;
            KFSMEditorWindow.OnSearchFieldValueChanged -= OnSearchFieldValueChanged;
            KFSMEditorWindow.OnSearchFieldValueChanged += OnSearchFieldValueChanged;

            KFSMEditorWindow.OnStateDisplayNameChange -= OnStateDisplayNameChange;
            KFSMEditorWindow.OnStateDisplayNameChange += OnStateDisplayNameChange;

            KFSMEditorWindow.OnStateIndexChange -= OnStateIndexChange;
            KFSMEditorWindow.OnStateIndexChange += OnStateIndexChange;
            KFSMEditorWindow.OnStateIndexChangedFinal -= OnStateIndexChangedFinal;
            KFSMEditorWindow.OnStateIndexChangedFinal += OnStateIndexChangedFinal;            

            KFSMEditorWindow.OnStateAdd -= OnStateAdd;
            KFSMEditorWindow.OnStateAdd += OnStateAdd;
            KFSMEditorWindow.OnStateAddedFinal -= OnStateAddedFinal;
            KFSMEditorWindow.OnStateAddedFinal += OnStateAddedFinal;

            KFSMEditorWindow.OnStateRemove -= OnStateRemove;
            KFSMEditorWindow.OnStateRemove += OnStateRemove;
            KFSMEditorWindow.OnStateRemovedFinal -= OnStateRemovedFinal;
            KFSMEditorWindow.OnStateRemovedFinal += OnStateRemovedFinal;

            KFSMEditorWindow.OnStateValueChange -= OnStateValueChange;
            KFSMEditorWindow.OnStateValueChange += OnStateValueChange;

            KFSMEditorWindow.OnStateTransitionConditionRemovedFinal -= OnStateTransitionConditionRemovedFinal;
            KFSMEditorWindow.OnStateTransitionConditionRemovedFinal += OnStateTransitionConditionRemovedFinal;
        }

        private void OnDisable()
        {
            KFSMEditorWindow.OnControllerChange -= OnControllerChange;
            KFSMEditorWindow.OnControllerChanged -= OnControllerChanged;
            KFSMEditorWindow.OnControllerChangedFinal -= OnControllerChangedFinal;

            KFSMEditorWindow.OnSelectedStateChange -= OnSelectedStateChange;

            KFSMEditorWindow.OnSelectedStateDeselect -= OnSelectedStateDeselect;

            KFSMEditorWindow.OnSearchFieldValueChange -= OnSearchFieldValueChange;
            KFSMEditorWindow.OnSearchFieldValueChanged -= OnSearchFieldValueChanged;

            KFSMEditorWindow.OnStateDisplayNameChange -= OnStateDisplayNameChange;

            KFSMEditorWindow.OnStateIndexChange -= OnStateIndexChange;
            KFSMEditorWindow.OnStateIndexChangedFinal -= OnStateIndexChangedFinal;

            KFSMEditorWindow.OnStateAdd -= OnStateAdd;
            KFSMEditorWindow.OnStateAddedFinal -= OnStateAddedFinal;

            KFSMEditorWindow.OnStateRemove -= OnStateRemove;
            KFSMEditorWindow.OnStateRemovedFinal -= OnStateRemovedFinal;

            KFSMEditorWindow.OnStateValueChange -= OnStateValueChange;

            KFSMEditorWindow.OnStateTransitionConditionRemovedFinal -= OnStateTransitionConditionRemovedFinal;
        }

        private void OnControllerChange(KFSMController newController)
        {
            States.Clear();
            StatesFiltered.Clear();
        }

        private void OnControllerChanged(KFSMController newController)
        {
            if (newController == null) return;

            var states = newController.GetStateContainers();
            States.AddRange(states);
            StatesFiltered.AddRange(States);
        }

        private void OnControllerChangedFinal()
        {
            ScriptableObjectUtils.MarkDirty(this);
        }

        private void OnSelectedStateChange(SKFSMStateContainer stateContainer)
        {
            SelectedState = stateContainer.State;
        }

        private void OnSelectedStateDeselect()
        {
            SelectedState = null;
        }

        private void OnSearchFieldValueChange(string searchText)
        {
            SearchFieldText = searchText;
        }

        private void OnSearchFieldValueChanged(string searchText)
        {
            var filteredStates = States.Where(s => IsFilterMatchState(s)).ToList();
            StatesFiltered.Clear();
            foreach (var filter in filteredStates)
            {
                StatesFiltered.Add(filter);
            }
        }

        private void OnStateDisplayNameChange(SKFSMStateContainer stateContainer, string newDisplayName)
        {
            var index = States.IndexOf(stateContainer);
            var filteredIndex = StatesFiltered.IndexOf(stateContainer);

            if (index != -1)
            {
                var updatedContainer = States[index];
                updatedContainer.DisplayName = newDisplayName;
                States[index] = updatedContainer;

                if (States[index].State != null)
                {
                    States[index].State.DisplayName = newDisplayName;
                    ScriptableObjectUtils.MarkDirty(States[index].State);
                }

                ParentData.Controller.SetDisplayName(index, updatedContainer);
            }

            if (filteredIndex != -1)
            {
                var updatedContainer = StatesFiltered[filteredIndex];
                updatedContainer.DisplayName = newDisplayName;
                StatesFiltered[filteredIndex] = updatedContainer;
            }

            ScriptableObjectUtils.MarkDirty(this, ParentData.Controller);
        }

        private void OnStateIndexChange(int oldIndex, int newIndex)
        {
            if (oldIndex < 0 || newIndex < 0 || oldIndex >= States.Count || newIndex >= States.Count)
            {
                Debug.LogWarning($"[KFSMEWData] Invalid state index change from {oldIndex} to {newIndex}. Index out of range.");
                return;
            }

            var oldIndexParameter = States[oldIndex];
            States.RemoveAt(oldIndex);
            States.Insert(newIndex, oldIndexParameter);

            ParentData.Controller.ReorderStates(oldIndex, newIndex);
        }

        private void OnStateIndexChangedFinal()
        {
            ScriptableObjectUtils.MarkDirty(this, ParentData.Controller);
        }

        private void OnStateAdd(SKFSMStateContainer stateContainer)
        {
            States.Add(stateContainer);
            if (IsFilterMatchState(stateContainer)) 
                StatesFiltered.Add(stateContainer);

            ParentData.AddState(stateContainer);
        }

        private void OnStateAddedFinal()
        {
            ScriptableObjectUtils.MarkDirty(this, ParentData.Controller);
        }

        private void OnStateRemove(SKFSMStateContainer stateContainer)
        {
            if (StatesFiltered.Contains(stateContainer)) StatesFiltered.Remove(stateContainer);

            States.Remove(stateContainer);
            ParentData.Controller.RemoveState(stateContainer);
        }

        private void OnStateRemovedFinal()
        {
            ScriptableObjectUtils.MarkDirty(this, ParentData.Controller);
        }

        private void OnStateValueChange(SKFSMStateContainer stateContainer, KFSMState newState)
        {
            var index = States.IndexOf(stateContainer);
            var updatedContainer = stateContainer;

            if (index != -1)
            {
                updatedContainer = States[index];
                updatedContainer.State = newState;

                States[index] = updatedContainer;
                ParentData.Controller.SetState(stateContainer, updatedContainer);
                KFSMEditorWindow.ChangeSelectedState(updatedContainer);
            }

            index = StatesFiltered.IndexOf(stateContainer);
            if (index != -1)
            {
                StatesFiltered[index] = updatedContainer;
            }
            ScriptableObjectUtils.MarkDirty(this, ParentData.Controller);
        }

        private void OnStateTransitionConditionRemovedFinal()
        {
            ScriptableObjectUtils.MarkDirty(SelectedState);
        }

        private bool IsFilterMatchState(SKFSMStateContainer stateContainer)
        {
            return stateContainer.DisplayName.ToLower().Contains(SearchFieldText.ToLower())
                || (stateContainer.State != null && stateContainer.State.StateName.ToLower().Contains(SearchFieldText.ToLower()));
        }
    }
}
