using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVIPSVIPSVCItemList : KFSMEWSVIPSVIPSVCItemGroup
    {
        internal ListView List { get; private set; }

        internal VisualElement ButtonContainer { get; private set; }
        internal Button AddButton { get; private set; }
        internal Button RemoveButton { get; private set; }  

        internal KFSMEWSVIPSVIPSVCItemList() : this("List") { }

        internal KFSMEWSVIPSVIPSVCItemList(string nameText) : base(nameText)
        {
            List = new();
            List.AddToClassList("kfsmewsvipsvc-item-list");

            ButtonContainer = new();
            ButtonContainer.AddToClassList("kfsmewsvipsvc-item-list-button-container");
            ButtonContainer.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedEvent);

            AddButton = new();
            AddButton.AddToClassList("kfsmewsvipsvc-item-list-button");
            AddButton.text = "+";
            ButtonContainer.AddToClassList("Add");
            RemoveButton = new();
            RemoveButton.AddToClassList("kfsmewsvipsvc-item-list-button");
            RemoveButton.text = "-";

            ButtonContainer.Add(AddButton);
            ButtonContainer.Add(RemoveButton);

            ContentElement.Add(List);
            ContentElement.Add(ButtonContainer);
        }

        private void OnGeometryChangedEvent(GeometryChangedEvent evt)
        {
            float height = ButtonContainer.resolvedStyle.height;
            ButtonContainer.style.bottom = -height;
            ContentElement.style.marginBottom = height;
        }

        internal void RefreshList()
        {
            List.RefreshItems();
            List.Rebuild();
        }
    }
}
