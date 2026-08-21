using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    [UxmlElement]
    public partial class KFSMEWContainer : VisualElement
    {
        public KFSMEWContainer()
        {
            AddToClassList("kfsmew-container");

            KFSMEWHeader header = new();
            KFSMEWSplitView splitView = new();

            Add(header);
            Add(splitView);
        }
    }
}
