using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVSPTSearchFieldState : ToolbarSearchField
    {
        internal KFSMEWSVSPTSearchFieldState()
        {
            AddToClassList("kfsmewsvspt-search-field-state");

            placeholderText = "State Name";

            this.RegisterValueChangedCallback(evt => KFSMEditorWindow.ChangeSearchFieldValue(evt.newValue));
        }
    }
}
