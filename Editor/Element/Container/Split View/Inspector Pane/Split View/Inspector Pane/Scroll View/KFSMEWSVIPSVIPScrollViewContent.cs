using Codice.Client.BaseCommands.Merge.Xml;
using KuzuStudios.KBlackboard;
using KuzuStudios.Kutils;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using static KuzuStudios.KFSM.Editor.KFSMEWStateTransitionData;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVIPSVIPScrollViewContent : ScrollView
    {
        private readonly List<KFSMEWSVIPSVIPSVCItemGroup> _groups = new();

        private SKFSMStateContainer _selectedStateContainer;
        private SKFSMStateTransitionContainer _selectedStateTransitionContainer;

        internal KFSMEWSVIPSVIPScrollViewContent()
        {
            AddToClassList("kfsmewsvip-scroll-view-content");

            KBlackboardController blackboardValue = KFSMEWData.Instance.Controller.Blackboard;

            AddGroup("KFSM Settings", 
                new KFSMEWSVIPSVIPSVCIFScriptableObject<KFSMController>("Controller", KFSMEWData.Instance.Controller, true, ChangeBlackboard)
            );            

            KFSMEditorWindow.OnSelectedStateChanged += OnSelectedStateChanged;
            KFSMEditorWindow.OnSelectedStateDeselectedFinal += ClearGroups;
            KFSMEditorWindow.OnSelectedStateTransitionChanged += OnSelectedStateTransitionChanged;
            KFSMEditorWindow.OnSelectedStateTransitionDeselectedFinal += ClearTransition;
        }

        private void OnSelectedStateChanged(SKFSMStateContainer container)
        {
            ClearGroups();

            _selectedStateContainer = container;

            AddGroup("Container Settings",
                new KFSMEWSVIPSVIPSVCIFString("Display Name", _selectedStateContainer.DisplayName, false, null),
                new KFSMEWSVIPSVIPSVCIFScriptableObject<KFSMState>("State", KFSMEWData.Instance.StateData.SelectedState, true, ChangeStateValue)
            );

            if (_selectedStateContainer.State == null) return;

            string groupName = $"State Settings";

            Type type = _selectedStateContainer.State.GetType();

            FieldInfo[] fieldInfo = type.GetFields(BindingFlags.Instance | BindingFlags.Public);

            List<KFSMEWSVIPSVIPSVCItemBase> items = new();
            List<KFSMEWSVIPSVIPSVCItemBase> readonlyItems = new();
            foreach (FieldInfo field in fieldInfo)
            {
                if (field.Name == "Transitions" || field.Name == "Controller" || field.Name == "DisplayName") continue;

                var fieldType = field.FieldType;

                bool toggleOff = field.Name == "DisplayName" || field.Name == "StartTime" || field.Name == "EndTime" ||
                    field.Name == "IsExitingState" || field.Name == "IsAnimationFinished";

                bool enabledOn = !toggleOff;

                KFSMEWSVIPSVIPSVCItemBase item = fieldType switch
                {
                    Type t when t == typeof(bool) => new KFSMEWSVIPSVIPSVCIFBool(field.Name, (bool)field.GetValue(_selectedStateContainer.State), enabledOn, (evt) =>
                    {
                        SetValue(field, evt.newValue, _selectedStateContainer.State);
                    }),
                    Type t when t == typeof(Color) => new KFSMEWSVIPSVIPSVCIFColor(field.Name, (Color)field.GetValue(_selectedStateContainer.State), enabledOn, (evt) =>
                    {
                        SetValue(field, evt.newValue, _selectedStateContainer.State);
                    }),
                    Type t when t == typeof(float) => new KFSMEWSVIPSVIPSVCIFFloat(field.Name, (float)field.GetValue(_selectedStateContainer.State), enabledOn, (evt) =>
                    {
                        SetValue(field, evt.newValue, _selectedStateContainer.State);
                    }),
                    Type t when t == typeof(GameObject) => new KFSMEWSVIPSVIPSVCIFGameObject(field.Name, (GameObject)field.GetValue(_selectedStateContainer.State), enabledOn, (evt) =>
                    {
                        SetValue(field, evt.newValue, _selectedStateContainer.State);
                    }),
                    Type t when t == typeof(int) => new KFSMEWSVIPSVIPSVCIFInt(field.Name, (int)field.GetValue(_selectedStateContainer.State), enabledOn, (evt) =>
                    {
                        SetValue(field, evt.newValue, _selectedStateContainer.State);
                    }),
                    Type t when t == typeof(ScriptableObject) => new KFSMEWSVIPSVIPSVCIFScriptableObject<ScriptableObject>(field.Name, (ScriptableObject)field.GetValue(_selectedStateContainer.State), enabledOn, (evt) =>
                    {
                        SetValue(field, evt.newValue, _selectedStateContainer.State);
                    }),
                    Type t when t == typeof(string) => new KFSMEWSVIPSVIPSVCIFString(field.Name, (string)field.GetValue(_selectedStateContainer.State), enabledOn, (evt) =>
                    {
                        SetValue(field, evt.newValue, _selectedStateContainer.State);
                    }),
                    Type t when t == typeof(Transform) => new KFSMEWSVIPSVIPSVCIFTransform(field.Name, (Transform)field.GetValue(_selectedStateContainer.State), enabledOn, (evt) =>
                    {
                        SetValue(field, evt.newValue, _selectedStateContainer.State);
                    }),
                    Type t when t == typeof(Vector2) => new KFSMEWSVIPSVIPSVCIFVector2(field.Name, (Vector2)field.GetValue(_selectedStateContainer.State), enabledOn, (evt) =>
                    {
                        SetValue(field, evt.newValue, _selectedStateContainer.State);
                    }),
                    Type t when t == typeof(Vector3) => new KFSMEWSVIPSVIPSVCIFVector3(field.Name, (Vector3)field.GetValue(_selectedStateContainer.State), enabledOn, (evt) =>
                    {
                        SetValue(field, evt.newValue, _selectedStateContainer.State);
                    }),
                    _ => throw new NullReferenceException($"Field type {fieldType} is not supported for field {field.Name} in state {_selectedStateContainer.DisplayName}.")
                };

                if (toggleOff) readonlyItems.Add(item);
                else items.Add(item); 
            }

            KFSMEWSVIPSVIPSVCItemGroup group = new(title: "Default", items: readonlyItems.ToArray());
            // // Toggle the group on to show the default values
            //using (ClickEvent clickEvent = ClickEvent.GetPooled())
            //{
            //    clickEvent.target = group.ToggleButton;
            //    group.ToggleButton.SendEvent(clickEvent);
            //}

            items.Insert(0, group);
            AddGroup(groupName, items.ToArray());
        }

        private void OnSelectedStateTransitionChanged(SKFSMStateTransitionContainer container)
        {
            ClearTransition();
            if (_selectedStateContainer.DisplayName == string.Empty)
            {
                var group = _groups[^1];
                Remove(group);
                _groups.Remove(group);
            }

            _selectedStateTransitionContainer = container;

            if (_selectedStateTransitionContainer.Transition == null) return;

            List<KFSMEWSVIPSVIPSVCItemBase> items = new();

            Type type = _selectedStateTransitionContainer.Transition.GetType();

            FieldInfo[] fieldInfo = type.GetFields(BindingFlags.Instance | BindingFlags.Public);

            foreach (FieldInfo field in fieldInfo)
            {
                if (field.Name == "Controller") continue;

                KFSMEWSVIPSVIPSVCItemBase tempItem = null;

                if (field.Name == "Conditions")
                {
                    tempItem = new KFSMEWSVIPSVIPSVCILCondition($"Conditions");
                }
                else
                {
                    var fieldType = field.FieldType;

                    tempItem = fieldType switch
                    {
                        Type t when t == typeof(bool) => new KFSMEWSVIPSVIPSVCIFBool(field.Name, (bool)field.GetValue(_selectedStateTransitionContainer.Transition), false, null),
                        Type t when t == typeof(Color) => new KFSMEWSVIPSVIPSVCIFColor(field.Name, (Color)field.GetValue(_selectedStateTransitionContainer.Transition), false, null),
                        Type t when t == typeof(float) => new KFSMEWSVIPSVIPSVCIFFloat(field.Name, (float)field.GetValue(_selectedStateTransitionContainer.Transition), false, null),
                        Type t when t == typeof(GameObject) => new KFSMEWSVIPSVIPSVCIFGameObject(field.Name, (GameObject)field.GetValue(_selectedStateTransitionContainer.Transition), false, null),
                        Type t when t == typeof(int) => new KFSMEWSVIPSVIPSVCIFInt(field.Name, (int)field.GetValue(_selectedStateTransitionContainer.Transition), false, null),
                        Type t when t == typeof(ScriptableObject) => new KFSMEWSVIPSVIPSVCIFScriptableObject<ScriptableObject>(field.Name, (ScriptableObject)field.GetValue(_selectedStateTransitionContainer.Transition), false, null),
                        Type t when t == typeof(string) => new KFSMEWSVIPSVIPSVCIFString(field.Name, (string)field.GetValue(_selectedStateTransitionContainer.Transition), false, null),
                        Type t when t == typeof(Transform) => new KFSMEWSVIPSVIPSVCIFTransform(field.Name, (Transform)field.GetValue(_selectedStateTransitionContainer.Transition), false, null),
                        Type t when t == typeof(Vector2) => new KFSMEWSVIPSVIPSVCIFVector2(field.Name, (Vector2)field.GetValue(_selectedStateTransitionContainer.Transition), false, null),
                        Type t when t == typeof(Vector3) => new KFSMEWSVIPSVIPSVCIFVector3(field.Name, (Vector3)field.GetValue(_selectedStateTransitionContainer.Transition), false, null),
                        _ => throw new NullReferenceException($"Field type {fieldType} is not supported for field {field.Name} in transition {_selectedStateTransitionContainer.DisplayName}.")
                    };
                }

                if (tempItem != null) items.Add(tempItem);
            }

            //items.Add(new KFSMEWSVIPSVIPSVCIGTransition());
            AddGroup("Transition", items.ToArray());
        }

        private void SetValue(FieldInfo field, object value, ScriptableObject so)
        {
            field.SetValue(so, value);
            ScriptableObjectUtils.MarkDirty(KFSMEWData.Instance, so);
        }

        private void ChangeBlackboard(ChangeEvent<UnityEngine.Object> evt)
        {
            KFSMEWData.Instance.Controller.Blackboard = (KBlackboardController)evt.newValue;

            ScriptableObjectUtils.MarkDirty(KFSMEWData.Instance, KFSMEWData.Instance.Controller, KFSMEWData.Instance.Controller.Blackboard);
        }

        private void ChangeStateValue(ChangeEvent<UnityEngine.Object> evt)
        {
            var newState = (KFSMState)evt.newValue;
            KFSMEditorWindow.ChangeStateValue(_selectedStateContainer, newState);

            var tmpCon = _selectedStateContainer;
            tmpCon.State = newState;
            _selectedStateContainer = tmpCon;
        }

        private void AddGroup(string groupName, params KFSMEWSVIPSVIPSVCItemBase[] items)
        {
            var group = new KFSMEWSVIPSVIPSVCItemGroup(title: groupName, items: items);
            _groups.Add(group);
            Add(group);
        }

        private void ClearGroups()
        {
            ClearUntil(1);
        }

        private void ClearTransition()
        {
            ClearUntil(3);
        }

        private void ClearUntil(int index)
        {
            while (_groups.Count > index)
            {
                var group = _groups[^1];
                _groups.RemoveAt(_groups.Count - 1);
                Remove(group);
            }
        }
    }
}
