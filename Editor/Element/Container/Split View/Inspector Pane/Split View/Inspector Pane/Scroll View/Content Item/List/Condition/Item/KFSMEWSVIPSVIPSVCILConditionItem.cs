using KuzuStudios.KBlackboard;
using KuzuStudios.Kutils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVIPSVIPSVCILConditionItem : VisualElement
    {
        internal DropdownField Parameter1DropdownField { get; private set; }
        internal DropdownField OperationDropdownField { get; private set; }
        internal DropdownField Parameter2DropdownField { get; private set; }

        internal IntegerField IntField { get; private set; }
        internal FloatField FloatField { get; private set; }
        internal Toggle BoolField { get; private set; }

        internal VisualElement Parameter2ThresholdField { get; private set; }

        internal VisualElement Parameter2Container { get; private set; }

        private KFSMCondition _condition;

        private KBlackboardController _blackboard;

        internal KFSMEWSVIPSVIPSVCILConditionItem()
        {
            AddToClassList("kfsmewsvipsvipsvcil-condition-item");
        }

        internal void Initialize(KFSMCondition condition)
        {
            _condition = condition;

            Parameter1DropdownField = new();
            Parameter1DropdownField.AddToClassList("kfsmewsvipsvipsvcil-condition-item-parameter1");
            Parameter1DropdownField.RegisterValueChangedCallback(OnParameter1DropdownFieldValueChanged);

            _blackboard = KFSMEWData.Instance.Controller.Blackboard;

            if (_blackboard == null)
                return;

            var parameter1 = _blackboard.GetParameter(_condition.Parameter1Name);
            var operation = _condition.OperationType.ToString();
            var parameter2 = _blackboard.GetParameter(_condition.Parameter2Name);

            UpdateParameter1DropdownField(parameter1);
            UpdateOperationDropdownField(operation, parameter1);
            UpdateParameter2Container(parameter2, parameter1.ParameterType);            
        }

        private void UpdateParameter1DropdownField(KBlackboardParameterBase parameter1)
        {
            if (Contains(Parameter1DropdownField)) Remove(Parameter1DropdownField);

            Parameter1DropdownField.value = parameter1.name;

            var listOfParameters = _blackboard.GetParameters(EKBlackboardParameterType.Float, EKBlackboardParameterType.Int, EKBlackboardParameterType.Bool);
            foreach (var param in listOfParameters)
            {
                string paramName = param.name;
                Parameter1DropdownField.choices.Add(paramName);
            }
            Add(Parameter1DropdownField);
        }

        private void UpdateOperationDropdownField(string operation, KBlackboardParameterBase parameter1)
        {           
            var listOfOperationNames = KFSMEWConditionData.GetAvailableOperations(parameter1.ParameterType).Select(o => o.ToString()).ToList();

            UpdateOperationDropdownField(listOfOperationNames);            
        }
        private void UpdateOperationDropdownField(List<string> listOfOperationNames)
        {
            if (Contains(OperationDropdownField)) Remove(OperationDropdownField);

            OperationDropdownField = new();
            OperationDropdownField.AddToClassList("kfsmewsvipsvipsvcil-condition-item-operation");
            OperationDropdownField.RegisterValueChangedCallback(OnOperationDropdownFieldValueChanged);

            OperationDropdownField.value = listOfOperationNames[0].ToString();

            foreach (var operationName in listOfOperationNames)
            {
                OperationDropdownField.choices.Add(operationName);
            }

            Insert(1, OperationDropdownField);
        }

        private void UpdateParameter2Container(KBlackboardParameterBase parameter2, EKBlackboardParameterType parameter1Type)
        {
            var listOfParameterNames = parameter1Type switch
            {
                EKBlackboardParameterType.Int => _blackboard.GetParameters(EKBlackboardParameterType.Int, EKBlackboardParameterType.Float).Select(p => p.name).ToList(),
                EKBlackboardParameterType.Float => _blackboard.GetParameters(EKBlackboardParameterType.Float, EKBlackboardParameterType.Int).Select(p => p.name).ToList(),
                EKBlackboardParameterType.Bool => _blackboard.GetParameters(EKBlackboardParameterType.Bool).Select(p => p.name).ToList(),
                _ => throw new NotImplementedException($"Unexpected Parameter Type: {parameter1Type}.")
            };
            listOfParameterNames.Insert(0, "None");

            UpdateParameter2Container(listOfParameterNames, parameter1Type);
        }

        private void UpdateParameter2Container(List<string> listOfParameterNames, EKBlackboardParameterType parameter1Type)
        {
            if (Contains(Parameter2Container)) Remove(Parameter2Container);

            Parameter2Container = new();
            Parameter2Container.AddToClassList("kfsmewsvipsvipsvcil-condition-item-parameter2-container");

            Parameter2DropdownField = new();
            Parameter2DropdownField.AddToClassList("kfsmewsvipsvipsvcil-condition-item-parameter2-container-item");
            Parameter2DropdownField.RegisterValueChangedCallback(OnParameter2DropdownFieldValueChanged);

            var paramName = _condition.Parameter2Name;
            Parameter2DropdownField.value = listOfParameterNames.Contains(paramName) ? paramName : "None";
            if (_condition.Parameter2Name != paramName) _condition.Parameter2Name = paramName;

            foreach (var param in listOfParameterNames)
            {
                string tmpParamName = param;

                if (tmpParamName == _condition.Parameter1Name) continue;

                Parameter2DropdownField.choices.Add(tmpParamName);
            }

            UpdateParameter2ContainerThreshold(paramName, parameter1Type);

            Parameter2Container.Insert(0, Parameter2DropdownField);

            Add(Parameter2Container);
        }

        private void UpdateParameter2ContainerThreshold(string paramName)
        {
            var param1Type = _blackboard.GetParameter(_condition.Parameter1Name).ParameterType;
            UpdateParameter2ContainerThreshold(paramName, param1Type);
        }

        private void UpdateParameter2ContainerThreshold(string paramName, EKBlackboardParameterType parameter1Type)
        {
            if (Parameter2Container.Contains(Parameter2ThresholdField)) Parameter2Container.Remove(Parameter2ThresholdField);

            Parameter2DropdownField.RemoveFromClassList("kfsmewsvipsvipsvcil-condition-item-parameter2-container-item-none");

            IntField = null;
            FloatField = null;
            BoolField = null;
            Parameter2ThresholdField = null;

            if (paramName == "None")
            {
                VisualElement tmpField;

                switch (parameter1Type)
                {
                    case EKBlackboardParameterType.Int:
                        tmpField = IntField = new();
                        IntField.value = Mathf.FloorToInt(_condition.Parameter2Threshold);
                        IntField.RegisterValueChangedCallback(OnIntFieldValueChanged);
                        break;
                    case EKBlackboardParameterType.Float:
                        tmpField = FloatField = new();
                        FloatField.value = _condition.Parameter2Threshold;
                        FloatField.RegisterValueChangedCallback(OnFloatFieldValueChanged);
                        break;
                    case EKBlackboardParameterType.Bool:
                        tmpField = BoolField = new Toggle();
                        BoolField.value = _condition.Parameter2Threshold != 0;
                        BoolField.RegisterValueChangedCallback(OnBoolFieldValueChanged);
                        break;
                    default: throw new NullReferenceException($"Unexpected Parameter Type: {parameter1Type}.");
                }

                tmpField.AddToClassList("kfsmewsvipsvipsvcil-condition-item-parameter2-container-item-threshold");
                Parameter2ThresholdField = new();
                Parameter2ThresholdField.Add(tmpField);
                Parameter2ThresholdField.AddToClassList("kfsmewsvipsvipsvcil-condition-item-parameter2-container-item-threshold");


                Parameter2DropdownField.AddToClassList("kfsmewsvipsvipsvcil-condition-item-parameter2-container-item-none");
                Parameter2Container.Add(Parameter2ThresholdField);
            }
        }

        private void OnParameter1DropdownFieldValueChanged(ChangeEvent<string> evt)
        {
            var oldValue = evt.previousValue;
            var newValue = evt.newValue;

            if (oldValue == newValue) return;

            var oldParam = KFSMEWData.Instance.Controller.Blackboard.GetParameter(oldValue);
            var param = KFSMEWData.Instance.Controller.Blackboard.GetParameter(newValue);

            if (oldParam.ParameterType != param.ParameterType)
            {
                var newOperationList = KFSMEWConditionData.GetAvailableOperations(param.ParameterType).Select(o => o.ToString()).ToList();
                if (!OperationDropdownField.choices.SequenceEqual(newOperationList))
                {
                    UpdateOperationDropdownField(newOperationList);
                }

                var newParameter2List= param.ParameterType switch
                {
                    EKBlackboardParameterType.Int => _blackboard.GetParameters(EKBlackboardParameterType.Int, EKBlackboardParameterType.Float).Select(p => p.name).ToList(),
                    EKBlackboardParameterType.Float => _blackboard.GetParameters(EKBlackboardParameterType.Float, EKBlackboardParameterType.Int).Select(p => p.name).ToList(),
                    EKBlackboardParameterType.Bool => _blackboard.GetParameters(EKBlackboardParameterType.Bool).Select(p => p.name).ToList(),
                    _ => throw new NotImplementedException($"Unexpected Parameter Type: {param.ParameterType}.")
                };
                newParameter2List.Insert(0, "None");
                newParameter2List.Remove(newValue);
                if (!Parameter2DropdownField.choices.SequenceEqual(newParameter2List))
                {
                    UpdateParameter2Container(newParameter2List, param.ParameterType);

                    if (_condition.Parameter2Name != Parameter1DropdownField.value)
                    { 
                        _condition.Parameter2Name = Parameter2DropdownField.value;
                    }
                }
            }

            Parameter1DropdownField.value = newValue;
            _condition.Parameter1Name = newValue;

            ScriptableObjectUtils.MarkDirty(_condition);
        }

        private void OnOperationDropdownFieldValueChanged(ChangeEvent<string> evt)
        {
            var oldValue = evt.previousValue;
            var newValue = evt.newValue;

            if (oldValue == newValue) return;

            OperationDropdownField.value = newValue;
            _condition.OperationType = Enum.Parse<EKFSMOperationType>(newValue);

            ScriptableObjectUtils.MarkDirty(_condition);
        }

        private void OnParameter2DropdownFieldValueChanged(ChangeEvent<string> evt)
        {
            var oldValue = evt.previousValue;
            var newValue = evt.newValue;

            if (oldValue == newValue) return;

            Parameter2DropdownField.value = newValue;
            _condition.Parameter2Name = newValue;

            UpdateParameter2ContainerThreshold(newValue);

            ScriptableObjectUtils.MarkDirty(_condition);
        }

        private void OnIntFieldValueChanged(ChangeEvent<int> evt)
        {
            var oldValue = evt.previousValue;
            var newValue = evt.newValue;

            ChangeParameterThreshrold(oldValue, newValue);
        }

        private void OnFloatFieldValueChanged(ChangeEvent<float> evt)
        {
            var oldValue = evt.previousValue;
            var newValue = evt.newValue;

            ChangeParameterThreshrold(oldValue, newValue);
        }

        private void OnBoolFieldValueChanged(ChangeEvent<bool> evt)
        {
            var oldValue = evt.previousValue ? 1 : 0;
            var newValue = evt.newValue ? 1 : 0;

            ChangeParameterThreshrold(oldValue, newValue);
        }       

        private void ChangeParameterThreshrold(float oldValue, float newValue)
        {
            if (oldValue == newValue) return;

            _condition.Parameter2Threshold = newValue;

            ScriptableObjectUtils.MarkDirty(_condition);
        }
    }
}
