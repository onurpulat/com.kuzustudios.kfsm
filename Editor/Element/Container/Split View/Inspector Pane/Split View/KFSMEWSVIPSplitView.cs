using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVIPSplitView : TwoPaneSplitView
    {
        internal KFSMEWSVIPSplitViewStateTransitionPane StateTransitionPane { get; private set; }
        internal KFSMEWSVIPSplitViewInspectorPane InspectorPane { get; private set; }

        internal KFSMEWSVIPSplitView()
        {
            AddToClassList("kfsmewsvip-split-view");

            //fixedPaneInitialDimension = 150;

            StateTransitionPane = new();
            InspectorPane = new();

            VisualElement resizer = this.Q(className: "unity-two-pane-split-view__dragline");
            resizer.pickingMode = PickingMode.Ignore;
            resizer.style.cursor = StyleKeyword.Null;

            CollapseChild(0); 

            Add(StateTransitionPane);
            Add(InspectorPane);

            KFSMEditorWindow.OnSelectedStateChange += OnSelectedStateChange;
            KFSMEditorWindow.OnSelectedStateDeselect += OnSelectedStateDeselect;
        }

        private void OnSelectedStateChange(SKFSMStateContainer container)
        {
            if (container.State == null) CollapseChild(0);
            else UnCollapse();
        }

        private void OnSelectedStateDeselect()
        {
            CollapseChild(0);
        }
    }
}
