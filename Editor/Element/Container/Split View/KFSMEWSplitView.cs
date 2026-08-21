using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSplitView : TwoPaneSplitView
    {
        internal KFSMEWSplitViewStatesPane StatesPane;
        internal KFSMEWSplitViewInspectorPane InspectorPane;

        internal KFSMEWSplitView()
        {
            AddToClassList("kfsmew-split-view");
            fixedPaneInitialDimension = 150;

            StatesPane = new KFSMEWSplitViewStatesPane();
            InspectorPane = new KFSMEWSplitViewInspectorPane();

            Add(StatesPane);
            Add(InspectorPane);
        }
    }
}
