using KuzuStudios.Kutils;
using System;
using UnityEditor.UIElements;
using UnityEngine;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVSPTButtonAddState : ToolbarButton
    {
        internal KFSMEWSVSPTButtonAddState()
        {
            AddToClassList("kfsmewsvspt-button-add-state");
            text = "+";

            clicked += OnClicked;
        }

        private void OnClicked()
        {
            var checkList = KFSMEWData.Instance.StateData.States.ConvertAll(stateContainer => stateContainer.DisplayName);
            var uniqueName = StringUtils.GetUniqueName(KFSMEWStateData.NEW_EMPTY_CONTAINER_DISPLAY_NAME, checkList);

            SKFSMStateContainer newContainer = new SKFSMStateContainer
            {
                DisplayName = uniqueName,
                State = null
            };

            KFSMEditorWindow.AddState(newContainer);
        }
    }
}
