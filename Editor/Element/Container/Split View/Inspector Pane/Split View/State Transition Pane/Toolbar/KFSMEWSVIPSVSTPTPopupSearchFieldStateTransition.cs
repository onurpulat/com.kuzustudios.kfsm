using System;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    // TODO: Change Name To KFSMEWSVIPSVSTPTTSearchFieldStateTransition
    internal partial class KFSMEWSVIPSVSTPTPopupSearchFieldStateTransition : ToolbarPopupSearchField
    {
        internal KFSMEWSVIPSVSTPTPopupSearchFieldStateTransition()
        {
            AddToClassList("kfsmewsvipsvstpt-popup-search-field-state-transition");

            placeholderText = "State Name";

            KFSMEditorWindow.OnSelectedStateChanged += OnSelectedStateChanged;
            this.RegisterValueChangedCallback(evt => KFSMEditorWindow.ChangePopupSearchFieldValue(evt.newValue));
        }

        private void OnSelectedStateChanged(SKFSMStateContainer container)
        {
            if (container.State == null) return;

            menu.ClearItems();

            var test = KFSMEWData.Instance.StateData.States.ToList();

            RegisterMenuItem("All");

            foreach (var sc in test)
            {
                if (string.IsNullOrEmpty(sc.DisplayName)) continue;
                else if (sc.DisplayName == container.DisplayName && sc.State == container.State) continue;

                var menuName = !string.IsNullOrEmpty(sc.DisplayName) ? sc.DisplayName : sc.State != null ? sc.State.name : string.Empty;

                if (string.IsNullOrEmpty(menuName)) continue;

                RegisterMenuItem(menuName);
            }

            value = string.Empty;
            KFSMEditorWindow.ChangePopupSearchFieldMenu("All");
            //KFSMEditorWindow.ChangePopupSearchFieldValue("");
        }


        private void RegisterMenuItem(string menuName)
        {
            menu.AppendAction
            (
                menuName,
                action =>
                {
                    KFSMEditorWindow.ChangePopupSearchFieldMenu(menuName);
                },
                action => GetActionStatus(menuName)
            );
        }

        private DropdownMenuAction.Status GetActionStatus(string menuName)
        {
            return KFSMEWData.Instance.StateTransitionData.StateTransitionPopupSearchFieldMenu == menuName ? DropdownMenuAction.Status.Checked : DropdownMenuAction.Status.Normal;
        }
    }
}
