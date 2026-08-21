using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVIPSplitViewStateTransitionPane : VisualElement
    {
        internal KFSMEWSVIPSVSTPToolbar StateTransitionToolbar { get; private set; }
        internal KFSMEWSVIPSVSTPListView StateTransitionListView { get; private set; }

        internal KFSMEWSVIPSplitViewStateTransitionPane()
        {
            AddToClassList("kfsmewsvip-split-view-state-transition-pane");

            StateTransitionToolbar = new();
            StateTransitionListView = new();

            Add(StateTransitionToolbar);
            Add(StateTransitionListView);
        }
    }
}
