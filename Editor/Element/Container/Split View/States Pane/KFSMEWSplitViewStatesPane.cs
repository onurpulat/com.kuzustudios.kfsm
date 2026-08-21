using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSplitViewStatesPane : VisualElement
    {
        internal KFSMEWSVStatesPaneToolbar StatesPaneToolbar { get; private set; }
        internal KFSMEWSVSPListView StatesPaneListView { get; private set; }

        internal KFSMEWSplitViewStatesPane()
        {
            AddToClassList("kfsmew-split-view-states-pane");

            StatesPaneToolbar = new();
            StatesPaneListView = new();

            Add(StatesPaneToolbar);
            Add(StatesPaneListView);
        }
    }
}
