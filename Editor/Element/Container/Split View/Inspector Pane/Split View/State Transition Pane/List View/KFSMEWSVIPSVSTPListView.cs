using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVIPSVSTPListView : ListView
    {
        internal KFSMEWSVIPSVSTPListView()
        {
            AddToClassList("kfsmewsvipsvstp-list-view");

            itemsSource = KFSMEWData.Instance.StateTransitionData.StateTransitionsFiltered;
            makeNoneElement = () => new Label("No State Transitions");
            makeItem = () => new KFSMEWSVIPSVSTPListViewItem();
            bindItem = BindItem;
            reorderable = false;
            virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            selectionChanged += OnSelectedStateTransitionChange;

            KFSMEditorWindow.OnSelectedStateChangedFinal += RefreshList;
            KFSMEditorWindow.OnStateTransitionAddedFinal += RefreshList;
            KFSMEditorWindow.OnPopupSearchFieldValueChangedFinal += RefreshList;
            KFSMEditorWindow.OnPopupSearchFieldMenuChangedFinal += RefreshList;
            KFSMEditorWindow.OnSelectedStateChangedFinal += DeselectList;
            KFSMEditorWindow.OnSelectedStateDeselectedFinal += DeselectList;
            KFSMEditorWindow.OnSelectedStateTransitionDeselectedFinal += DeselectList;
            RefreshList();
        }

        private void BindItem(VisualElement element, int index)
        {
            var stateTransitionContainer = KFSMEWData.Instance.StateTransitionData.StateTransitionsFiltered[index];
            var item = (KFSMEWSVIPSVSTPListViewItem)element;

            item.Initialize(stateTransitionContainer);
        }

        private void OnSelectedStateTransitionChange(IEnumerable<object> enumerable)
        {
            foreach (var item in enumerable)
            {
                var container = (KFSMEWStateTransitionData.SKFSMStateTransitionContainer)item;
                KFSMEditorWindow.ChangeSelectedStateTransition(container);
                break;
            }
        }

        private void DeselectList()
        {
            selectedIndex = -1;
        }

        private void RefreshList()
        {
            RefreshItems();
            Rebuild();
        }
    }
}
