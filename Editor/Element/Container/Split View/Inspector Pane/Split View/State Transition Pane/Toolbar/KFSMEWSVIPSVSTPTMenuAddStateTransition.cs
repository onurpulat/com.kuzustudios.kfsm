using KuzuStudios.Kutils;
using KuzuStudios.Kutils.SO;
using System;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;

namespace KuzuStudios.KFSM.Editor
{
    // TODO: Change Name To KFSMEWSVIPSVSTPTMenuAddStateTransition
    internal partial class KFSMEWSVIPSVSTPTMenuAddStateTransition : ToolbarMenu
    {
        internal KFSMEWSVIPSVSTPTMenuAddStateTransition()
        {
            AddToClassList("kfsmewsvipsvstpt-menu-add-state-transition");

            text = "+";

            KFSMEditorWindow.OnSelectedStateChanged += OnSelectedStateChanged;
        }

        private void OnSelectedStateChanged(SKFSMStateContainer container)
        {
            if (container.State == null) return;

            menu.ClearItems();

            var test = KFSMEWData.Instance.StateData.States.ToList();

            foreach (var sc in test)
            {
                if (string.IsNullOrEmpty(sc.DisplayName)) continue;
                else if (sc.DisplayName == container.DisplayName && sc.State == container.State) continue;

                var menuName = !string.IsNullOrEmpty(sc.DisplayName) ? sc.DisplayName : sc.State != null ? sc.State.name : string.Empty;

                if (string.IsNullOrEmpty(menuName)) continue;

                menu.AppendAction(menuName, action =>
                {                    
                    KFSMEditorWindow.AddStateTranstion(sc);
                });
            }
        }
    }
}
