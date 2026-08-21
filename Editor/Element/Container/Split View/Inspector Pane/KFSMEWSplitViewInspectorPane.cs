using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSplitViewInspectorPane : VisualElement
    {
        internal KFSMEWSVIPSplitView InspectorPaneSplitView { get; private set; }
        internal KFSMEWSVIPToolbar Toolbar { get; private set; }

        internal KFSMEWSplitViewInspectorPane()
        {
            AddToClassList("kfsmew-split-view-inspector-pane");

            InspectorPaneSplitView = new();
            Toolbar = new();

            Add(Toolbar);
            Add(InspectorPaneSplitView);
        }
    }
}
