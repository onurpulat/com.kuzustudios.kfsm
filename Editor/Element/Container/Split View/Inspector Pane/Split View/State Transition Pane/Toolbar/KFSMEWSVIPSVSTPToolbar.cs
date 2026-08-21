using UnityEditor.UIElements;
using UnityEngine;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVIPSVSTPToolbar : Toolbar
    {
        internal KFSMEWSVIPSVSTPTPopupSearchFieldStateTransition StateTransitionSearchField { get; private set; }
        internal KFSMEWSVIPSVSTPTMenuAddStateTransition MenuAdd { get; private set; }

        internal KFSMEWSVIPSVSTPToolbar()
        {
            AddToClassList("kfsmewsvip-svstp-toolbar");

            StateTransitionSearchField = new();
            MenuAdd = new();

            Add(StateTransitionSearchField);
            Add(new ToolbarSpacer());
            Add(MenuAdd);
        }
    }
}
