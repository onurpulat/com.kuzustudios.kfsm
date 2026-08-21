using Codice.Client.BaseCommands.Update;
using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using static KuzuStudios.KFSM.Editor.KFSMEWStateTransitionData;

namespace KuzuStudios.KFSM.Editor
{
    
    internal partial class KFSMEWSVIPToolbar : Toolbar
    {
        internal ToolbarBreadcrumbs Breadcrumbs { get; private set; }

        private readonly List<string> _breadcrumbsTexts = new();
        private readonly List<Action> _breadcrumbsActions= new();

        internal KFSMEWSVIPToolbar()
        {
            AddToClassList("kfsmewsvip-toolbar");

            KFSMEditorWindow.OnSelectedStateChanged += OnSelectedStateChanged;
            KFSMEditorWindow.OnSelectedStateDeselect += OnSelectedStateDeselect;
            KFSMEditorWindow.OnSelectedStateDeselectedFinal += UpdateItems;
            KFSMEditorWindow.OnSelectedStateTransitionChanged += OnSelectedStateTransitionChanged;
            KFSMEditorWindow.OnSelectedStateTransitionDeselect += OnSelectedStateTransitionDeselect;
            KFSMEditorWindow.OnSelectedStateTransitionDeselectedFinal += UpdateItems;

            Breadcrumbs = new();

            _breadcrumbsTexts.Add("States");
            _breadcrumbsActions.Add(() => KFSMEditorWindow.DeselectSelectedState());

            Add(Breadcrumbs);

            UpdateItems();
        }

        private void OnSelectedStateChanged(SKFSMStateContainer container)
        {
            RemoveItemUntil(1);
            AddItem(container.DisplayName, () => { });
            UpdateItems();
        }

        private void OnSelectedStateDeselect()
        {
            RemoveItemUntil(1);
        }

        private void OnSelectedStateTransitionChanged(SKFSMStateTransitionContainer container)
        {
            _breadcrumbsActions[_breadcrumbsTexts.Count - 1] = () => KFSMEditorWindow.DeselectSelectedStateTransition();

            RemoveItemUntil(2);
            AddItem(container.DisplayName, () => { });
            UpdateItems();
        }

        private void OnSelectedStateTransitionDeselect()
        {
            RemoveItemUntil(2);
        }

        private void RemoveItemUntil(int index)
        {
            while (_breadcrumbsTexts.Count > index)
            {
                RemoveItem(_breadcrumbsTexts.Count - 1);
            }
        }

        private void RemoveItem(int index)
        {
            _breadcrumbsTexts.RemoveAt(index);
            _breadcrumbsActions.RemoveAt(index);
        }

        private void AddItem(string name, Action action)
        {
            _breadcrumbsTexts.Add(name);
            _breadcrumbsActions.Add(action);
        }

        private void UpdateItems()
        {
            while(Breadcrumbs.childCount > 0)
            {
                Breadcrumbs.PopItem();
            }

            for (int i = 0; i < _breadcrumbsTexts.Count; i++)
            {
                var text = _breadcrumbsTexts[i];
                var action = _breadcrumbsActions[i];

                Breadcrumbs.PushItem(text, action);
            }
        }
    }
}
