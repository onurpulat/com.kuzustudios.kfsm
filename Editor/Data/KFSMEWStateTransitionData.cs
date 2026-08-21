using KuzuStudios.Kutils;
using KuzuStudios.Kutils.SO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KuzuStudios.KFSM.Editor
{
    public class KFSMEWStateTransitionData : ScriptableObject
    {
        [System.Serializable]
        public struct SKFSMStateTransitionContainer
        {
            public string DisplayName;
            public KFSMStateTransition Transition;
        }

        public KFSMEWData ParentData;

        public List<SKFSMStateTransitionContainer> StateTransitions = new();
        public List<SKFSMStateTransitionContainer> StateTransitionsFiltered = new();
        public SKFSMStateTransitionContainer SelectedStateTransition;

        public string StateTransitionPopupSearchFieldText = string.Empty;
        public string StateTransitionPopupSearchFieldMenu = "All";

        private void OnEnable()
        {
            KFSMEditorWindow.OnSelectedStateChange -= OnSelectedStateChange;
            KFSMEditorWindow.OnSelectedStateChange += OnSelectedStateChange;

            KFSMEditorWindow.OnSelectedStateDeselect -= OnSelectedStateDeselect;
            KFSMEditorWindow.OnSelectedStateDeselect += OnSelectedStateDeselect;

            KFSMEditorWindow.OnStateTransitionAdd -= OnStateTransitionAdd;
            KFSMEditorWindow.OnStateTransitionAdd += OnStateTransitionAdd;
            KFSMEditorWindow.OnStateTransitionAddedFinal -= OnStateTransitionAddedFinal;
            KFSMEditorWindow.OnStateTransitionAddedFinal += OnStateTransitionAddedFinal;

            KFSMEditorWindow.OnPopupSearchFieldValueChange -= OnPopupSearchFieldValueChange;
            KFSMEditorWindow.OnPopupSearchFieldValueChange += OnPopupSearchFieldValueChange;
            KFSMEditorWindow.OnPopupSearchFieldValueChanged -= OnPopupSearchFieldValueChanged;
            KFSMEditorWindow.OnPopupSearchFieldValueChanged += OnPopupSearchFieldValueChanged;

            KFSMEditorWindow.OnPopupSearchFieldMenuChange -= OnPopupSearchFieldMenuChange;
            KFSMEditorWindow.OnPopupSearchFieldMenuChange += OnPopupSearchFieldMenuChange;
            KFSMEditorWindow.OnPopupSearchFieldMenuChanged -= OnPopupSearchFieldMenuChanged;
            KFSMEditorWindow.OnPopupSearchFieldMenuChanged += OnPopupSearchFieldMenuChanged;

            KFSMEditorWindow.OnSelectedStateTransitionChange -= OnSelectedStateTransitionChange;
            KFSMEditorWindow.OnSelectedStateTransitionChange += OnSelectedStateTransitionChange;


            KFSMEditorWindow.OnStateTransitionConditionRemovedFinal -= OnStateTransitionConditionRemovedFinal;
            KFSMEditorWindow.OnStateTransitionConditionRemovedFinal += OnStateTransitionConditionRemovedFinal;
        }

        private void OnDisable()
        {
            KFSMEditorWindow.OnSelectedStateChange -= OnSelectedStateChange;

            KFSMEditorWindow.OnSelectedStateDeselect -= OnSelectedStateDeselect;

            KFSMEditorWindow.OnStateTransitionAdd -= OnStateTransitionAdd;
            KFSMEditorWindow.OnStateTransitionAddedFinal -= OnStateTransitionAddedFinal;

            KFSMEditorWindow.OnPopupSearchFieldValueChange -= OnPopupSearchFieldValueChange;
            KFSMEditorWindow.OnPopupSearchFieldValueChanged -= OnPopupSearchFieldValueChanged;

            KFSMEditorWindow.OnPopupSearchFieldMenuChange -= OnPopupSearchFieldMenuChange;
            KFSMEditorWindow.OnPopupSearchFieldMenuChanged -= OnPopupSearchFieldMenuChanged;

            KFSMEditorWindow.OnSelectedStateTransitionChange -= OnSelectedStateTransitionChange;

            KFSMEditorWindow.OnStateTransitionConditionRemovedFinal -= OnStateTransitionConditionRemovedFinal;
        }

        private void OnSelectedStateChange(SKFSMStateContainer stateContainer)
        {
            StateTransitions.Clear();
            StateTransitionsFiltered.Clear();

            if (stateContainer.State == null) return;

            var transitions = stateContainer.State.Transitions.ToList();
            foreach (var transition in transitions)
            {
                SKFSMStateTransitionContainer newCon = new()
                {
                    DisplayName = transition.TargetStateName,
                    Transition = transition,
                };

                StateTransitions.Add(newCon);
            }

            StateTransitionsFiltered.AddRange(StateTransitions);
        }

        private void OnSelectedStateDeselect()
        {
            StateTransitions.Clear();
            StateTransitionsFiltered.Clear();
        }

        private void OnStateTransitionAdd(SKFSMStateContainer transitionStateContainer)
        {
            if (ParentData.StateData.SelectedState == null) return;

            string baseName = $"Transition_{ParentData.StateData.SelectedState.DisplayName}_to_{transitionStateContainer.DisplayName}";
            var listOfNames = ParentData.StateData.SelectedState.Transitions.Select(t => t.name).ToList();
            var uniqueName = StringUtils.GetUniqueName(baseName, listOfNames);
            var transition = new ScriptableObjectFactory<KFSMStateTransition>()
                .SetName(uniqueName)
                .SetParentSO(ParentData.StateData.SelectedState)
                .Create();

            transition.TargetStateName = transitionStateContainer.DisplayName;
            transition.Controller = ParentData.Controller;

            var stateTransitionContainer = new SKFSMStateTransitionContainer
            {
                DisplayName = transition.TargetStateName,
                Transition = transition,
            };
            StateTransitions.Add(stateTransitionContainer);
            if (IsFilterMatchStateTransition(stateTransitionContainer)) 
                StateTransitionsFiltered.Add(stateTransitionContainer);

            ParentData.StateData.SelectedState.AddTransition(transition);

            ScriptableObjectUtils.MarkDirty(transition, ParentData.StateData.SelectedState);
        }

        private void OnStateTransitionAddedFinal()
        {
            ScriptableObjectUtils.MarkDirty(this, ParentData.Controller);
        }

        private void OnPopupSearchFieldValueChange(string newValue)
        {
            StateTransitionPopupSearchFieldText = newValue;
        }

        private void OnPopupSearchFieldValueChanged(string newValue)
        {
            var filteredStateTransition = StateTransitions.Where(st => IsFilterMatchStateTransition(st)).ToList();
            StateTransitionsFiltered.Clear();
            foreach (var filter in filteredStateTransition)
            {
                StateTransitionsFiltered.Add(filter);
            }
        }

        private void OnPopupSearchFieldMenuChange(string newMenu)
        {
            StateTransitionPopupSearchFieldMenu = newMenu;
        }

        private void OnPopupSearchFieldMenuChanged(string newMenu)
        {
            var filteredStateTransition = StateTransitions.Where(st => IsFilterMatchStateTransition(st)).ToList();
            StateTransitionsFiltered.Clear();
            foreach (var filter in filteredStateTransition)
            {
                StateTransitionsFiltered.Add(filter);
            }
        }

        private void OnSelectedStateTransitionChange(SKFSMStateTransitionContainer stateTransitionContainer)
        {
            SelectedStateTransition = stateTransitionContainer;
        }

        private void OnStateTransitionConditionRemovedFinal()
        {
            ScriptableObjectUtils.MarkDirty(this, SelectedStateTransition.Transition);
        }

        private bool IsFilterMatchStateTransition(SKFSMStateTransitionContainer stateTransitionContainer)
        {
            return IsFilter(stateTransitionContainer.DisplayName, StateTransitionPopupSearchFieldText, stateTransitionContainer.DisplayName) 
                || IsFilter(stateTransitionContainer.Transition.name, StateTransitionPopupSearchFieldText, stateTransitionContainer.DisplayName);
        }

        private bool IsFilter(string nameText, string searchText, string menuText)
        {
            return nameText.ToLower().Contains(searchText.ToLower()) 
                && ((StateTransitionPopupSearchFieldMenu != "All" && StateTransitionPopupSearchFieldMenu == menuText)
                || StateTransitionPopupSearchFieldMenu == "All");

            //return stateTransitionContainer.DisplayName.ToLower().Contains(StateTransitionPopupSearchFieldText.ToLower()) &&
            //        ((StateTransitionPopupSearchFieldMenu != "All" && StateTransitionPopupSearchFieldMenu == stateTransitionContainer.DisplayName)
            //        || StateTransitionPopupSearchFieldMenu == "All");
        }
    }
}
