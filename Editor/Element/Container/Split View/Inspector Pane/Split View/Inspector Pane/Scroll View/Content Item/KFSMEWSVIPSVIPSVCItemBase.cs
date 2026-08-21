using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVIPSVIPSVCItemBase : VisualElement
    {
        internal Label NameLabel { get; private set; }

        internal KFSMEWSVIPSVIPSVCItemBase() : this("New Item Base") { }
        internal KFSMEWSVIPSVIPSVCItemBase(string nameText)
        {
            AddToClassList("kfsmewsvipsvc-item-base");

            NameLabel = new Label(nameText);

            Add(NameLabel);
        }
    }
}
