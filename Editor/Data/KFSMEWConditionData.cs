using KuzuStudios.KBlackboard;
using KuzuStudios.Kutils;
using KuzuStudios.Kutils.SO;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static KuzuStudios.KFSM.Editor.KFSMEWStateTransitionData;

namespace KuzuStudios.KFSM.Editor
{
    public class KFSMEWConditionData : ScriptableObject
    {
        public KFSMEWData ParentData;

        public List<KFSMCondition> Conditions = new();

        public int SelectedConditionIndex = -1;

        private void OnEnable()
        {
            KFSMEditorWindow.OnSelectedStateTransitionChanged -= OnSelectedStateTransitionChanged;
            KFSMEditorWindow.OnSelectedStateTransitionChanged += OnSelectedStateTransitionChanged;
            KFSMEditorWindow.OnSelectedStateTransitionChangedFinal -= OnSelectedStateTransitionChangedFinal;
            KFSMEditorWindow.OnSelectedStateTransitionChangedFinal += OnSelectedStateTransitionChangedFinal;

            KFSMEditorWindow.OnStateTransitionConditionCreate -= OnStateTransitionConditionCreate;
            KFSMEditorWindow.OnStateTransitionConditionCreate += OnStateTransitionConditionCreate;

            KFSMEditorWindow.OnStateTransitionConditionRemove -= OnStateTransitionConditionRemove;
            KFSMEditorWindow.OnStateTransitionConditionRemove += OnStateTransitionConditionRemove;
            KFSMEditorWindow.OnStateTransitionConditionRemoved -= OnStateTransitionConditionRemoved;
            KFSMEditorWindow.OnStateTransitionConditionRemoved += OnStateTransitionConditionRemoved;
            KFSMEditorWindow.OnStateTransitionConditionRemovedFinal -= OnStateTransitionConditionRemovedFinal;
            KFSMEditorWindow.OnStateTransitionConditionRemovedFinal += OnStateTransitionConditionRemovedFinal;

            KFSMEditorWindow.OnSelectedConditionIndexChange -= OnSelectedConditionIndexChange;
            KFSMEditorWindow.OnSelectedConditionIndexChange += OnSelectedConditionIndexChange;

            KFSMEditorWindow.OnSelectedConditionIndexChangedFinal -= OnSelectedConditionIndexChangedFinal;
            KFSMEditorWindow.OnSelectedConditionIndexChangedFinal += OnSelectedConditionIndexChangedFinal;
        }

        private void OnDisable()
        {
            KFSMEditorWindow.OnSelectedStateTransitionChanged -= OnSelectedStateTransitionChanged;
            KFSMEditorWindow.OnSelectedStateTransitionChangedFinal -= OnSelectedStateTransitionChangedFinal;

            KFSMEditorWindow.OnStateTransitionConditionCreate -= OnStateTransitionConditionCreate;

            KFSMEditorWindow.OnStateTransitionConditionRemove -= OnStateTransitionConditionRemove;
            KFSMEditorWindow.OnStateTransitionConditionRemoved -= OnStateTransitionConditionRemoved;
            KFSMEditorWindow.OnStateTransitionConditionRemovedFinal -= OnStateTransitionConditionRemovedFinal;

            KFSMEditorWindow.OnSelectedConditionIndexChange -= OnSelectedConditionIndexChange;
            KFSMEditorWindow.OnSelectedConditionIndexChangedFinal -= OnSelectedConditionIndexChangedFinal;
        }

        private void OnSelectedStateTransitionChanged(SKFSMStateTransitionContainer stateTransitionContainer)
        {
            SelectedConditionIndex = -1;
            Conditions = stateTransitionContainer.Transition.Conditions;
        }

        private void OnSelectedStateTransitionChangedFinal()
        {
            ScriptableObjectUtils.MarkDirty(this);
        }

        private void OnStateTransitionConditionCreate()
        {
            var baseName = $"Condition {ParentData.StateData.SelectedState.DisplayName} to {ParentData.StateTransitionData.SelectedStateTransition.Transition.TargetStateName}";
            var nameList = Conditions.Select(c => c.name).ToList();
            var uniqueName = StringUtils.GetUniqueName(baseName, nameList);
            var condition = new ScriptableObjectFactory<KFSMCondition>()
                .SetName(uniqueName)
                .SetParentSO(ParentData.StateData.SelectedState)
                .Create();

            var blackboard = ParentData.Controller.Blackboard;
            var firstParameter = blackboard.GetFirstParameterType(EKBlackboardParameterType.Float, EKBlackboardParameterType.Int, EKBlackboardParameterType.Bool);
            condition.Parameter1Name = firstParameter == null ? "Create New Parameter (Float, Int, Bool)" : firstParameter.name;
            condition.Parameter2Name = EKBlackboardParameterType.None.ToString();
            condition.Parameter2Threshold = 0;
            condition.OperationType = EKFSMOperationType.Equal;

            Conditions.Add(condition);

            KFSMEditorWindow.StateConditionAdd(condition);
            ScriptableObjectUtils.MarkDirty(this, condition);
        }

        private void OnStateTransitionConditionRemove()
        {
            if (Conditions.Count == 0) return;

            var index = SelectedConditionIndex == -1 ? Conditions.Count - 1 : SelectedConditionIndex;
            var condition = Conditions[index];

            ScriptableObjectUtils.DeleteFromScriptableObject(condition);
        }

        private void OnStateTransitionConditionRemoved()
        {
            if (Conditions.Count == 0) return;

            var index = SelectedConditionIndex == -1 ? Conditions.Count - 1 : SelectedConditionIndex;

            Conditions.RemoveAt(index);
        }

        private void OnStateTransitionConditionRemovedFinal()
        {
            SelectedConditionIndex = -1;
            ScriptableObjectUtils.MarkDirty(this);
            AssetDatabase.SaveAssets();
        }

        private void OnSelectedConditionIndexChange(int index)
        {
            SelectedConditionIndex = index;           
        }

        private void OnSelectedConditionIndexChangedFinal()
        {
            ScriptableObjectUtils.MarkDirty(this);
        }

        public static readonly List<EKFSMOperationType> AvailableIntOperations = new()
        {
            EKFSMOperationType.Equal,
            EKFSMOperationType.NotEqual,
            EKFSMOperationType.GreaterThan,
            EKFSMOperationType.LessThan,
            EKFSMOperationType.GreaterThanOrEqual,
            EKFSMOperationType.LessThanOrEqual
        };
        public static readonly List<EKFSMOperationType> AvailableFloatOperations = new()
        {
            EKFSMOperationType.GreaterThan,
            EKFSMOperationType.LessThan,
            EKFSMOperationType.GreaterThanOrEqual,
            EKFSMOperationType.LessThanOrEqual
        };
        public static readonly List<EKFSMOperationType> AvailableBoolOperations = new()
        {
            EKFSMOperationType.Equal,
            EKFSMOperationType.NotEqual
        };
        public static List<EKFSMOperationType> GetAvailableOperations(EKBlackboardParameterType parameterType)
            => parameterType switch
            {
                EKBlackboardParameterType.Int => AvailableIntOperations,
                EKBlackboardParameterType.Float => AvailableFloatOperations,
                EKBlackboardParameterType.Bool => AvailableBoolOperations,
                _ => throw new NotImplementedException($"Unexpected Parameter Type: {parameterType}.")
            };
    }
}
