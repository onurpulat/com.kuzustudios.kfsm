using KuzuStudios.KBlackboard;
using KuzuStudios.Kutils;
using KuzuStudios.Kutils.SO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace KuzuStudios.KFSM.Editor
{
    public class KFSMEWData : SingletonScriptableObject<KFSMEWData>
    {
        public KFSMController Controller;
        public KFSMEWStateData StateData;
        public KFSMEWStateTransitionData StateTransitionData;
        public KFSMEWConditionData ConditionData;

        protected override void OnCreate()
        {
            if (StateData == null)
            { 
                StateData = new ScriptableObjectFactory<KFSMEWStateData>()
                    .SetName("KFSMEW State Data")
                    .SetParentSO(this)
                    .Create();
                StateData.ParentData = this;
                ScriptableObjectUtils.MarkDirty(this);
            }

            if (StateTransitionData == null)
            {
                StateTransitionData = new ScriptableObjectFactory<KFSMEWStateTransitionData>()
                    .SetName("KFSMEW State Transition Data")
                    .SetParentSO(this)
                    .Create();
                StateTransitionData.ParentData = this;
                ScriptableObjectUtils.MarkDirty(this);
            }

            if (ConditionData == null)
            {
                ConditionData = new ScriptableObjectFactory<KFSMEWConditionData>()
                    .SetName("KFSMEW Condition Data")
                    .SetParentSO(this)
                    .Create();
                ConditionData.ParentData = this;
                ScriptableObjectUtils.MarkDirty(this);
            }
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            OnCreate();
            if (StateData.ParentData == null)
            {
                StateData.ParentData = this;
                ScriptableObjectUtils.MarkDirty(StateData);
            }
            if (StateTransitionData.ParentData == null)
            {
                StateTransitionData.ParentData = this;
                ScriptableObjectUtils.MarkDirty(StateTransitionData);
            }
            if (ConditionData.ParentData == null)
            {
                ConditionData.ParentData = this;
                ScriptableObjectUtils.MarkDirty(ConditionData);
            }
#endif

            KFSMEditorWindow.OnKFSMNameChanged -= OnKFSMNameChanged;
            KFSMEditorWindow.OnKFSMNameChanged += OnKFSMNameChanged;
        }

        private void OnDisable()
        {           
            KFSMEditorWindow.OnKFSMNameChanged -= OnKFSMNameChanged;            
        }        

        private void OnKFSMNameChanged(string newName)
        {
            ScriptableObjectUtils.ChangeScriptableObjectName(Controller, newName);
        }

        internal void AddState(SKFSMStateContainer stateContainer)
        {
            Controller.AddState(stateContainer);
        }
    }
}
