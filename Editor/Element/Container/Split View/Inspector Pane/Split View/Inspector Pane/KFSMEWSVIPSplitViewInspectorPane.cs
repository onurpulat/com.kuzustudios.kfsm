using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVIPSplitViewInspectorPane : VisualElement
    {
        internal KFSMEWSVIPSVIPScrollViewContent ContentScrollView { get; private set; }

        internal KFSMEWSVIPSplitViewInspectorPane()
        {
            AddToClassList("kfsmewsvip-split-view-inspector-pane");

            ContentScrollView = new KFSMEWSVIPSVIPScrollViewContent();
            Add(ContentScrollView);
        }
    }
}
