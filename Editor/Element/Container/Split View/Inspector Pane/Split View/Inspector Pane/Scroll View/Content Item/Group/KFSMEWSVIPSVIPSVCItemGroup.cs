using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVIPSVIPSVCItemGroup : KFSMEWSVIPSVIPSVCItemBase
    {
        protected const string _toggle_active_text = "▼";
        protected const string _toggle_inactive_text = "◀";

        internal VisualElement HeaderElement { get; private set; }
        internal Button ToggleButton {  get; private set; }
        internal VisualElement ContentElement { get; private set; }

        internal KFSMEWSVIPSVIPSVCItemGroup() : this("Group") { }

        internal KFSMEWSVIPSVIPSVCItemGroup(string title, bool toggleOn = true, params KFSMEWSVIPSVIPSVCItemBase[] items) : base(title)
        {
            AddToClassList("kfsmewsvipsvc-item-group");
            Remove(NameLabel);
            NameLabel.AddToClassList("kfsmewsvipsvc-item-group-header-name");

            ToggleButton = new();
            ToggleButton.AddToClassList("kfsmewsvipsvc-item-group-header-toggle");
            ToggleButton.text = _toggle_active_text;
            ToggleButton.clicked += Toggle;

            HeaderElement = new();
            HeaderElement.AddToClassList("kfsmewsvipsvc-item-group-header");

            HeaderElement.Add(NameLabel);
            HeaderElement.Add(ToggleButton);

            ContentElement = new();
            ContentElement.AddToClassList("kfsmewsvipsvc-item-group-content");

            Add(HeaderElement);
            Add(ContentElement);
            foreach (var item in items)
            {
                ContentElement.Add(item);
            }

            if (!toggleOn) Toggle();
        }

        protected virtual void Toggle()
        {
            var shouldOpen = ToggleButton.text == _toggle_inactive_text;
            
            ContentElement.style.display = shouldOpen ? DisplayStyle.Flex : DisplayStyle.None;
            ToggleButton.text = shouldOpen ? _toggle_active_text : _toggle_inactive_text;
        }
    }
}
