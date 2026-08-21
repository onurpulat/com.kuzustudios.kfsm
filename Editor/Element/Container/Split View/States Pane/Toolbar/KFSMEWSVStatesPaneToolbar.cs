using UnityEditor.UIElements;
using UnityEngine;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVStatesPaneToolbar : Toolbar
    {
        internal KFSMEWSVSPTSearchFieldState StatesSearchField { get; private set; }
        internal KFSMEWSVSPTButtonAddState ButtonAdd { get; private set; }

        internal KFSMEWSVStatesPaneToolbar()
        {
            AddToClassList("kfsmewsv-states-pane-toolbar");

            StatesSearchField = new();
            ButtonAdd = new();

            Add(StatesSearchField);
            Add(new ToolbarSpacer());
            Add(ButtonAdd);
        }
    }
}
