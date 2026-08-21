using KuzuStudios.KBlackboard;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVSPListView : ListView
    {
        internal KFSMEWSVSPListView()
        {
            AddToClassList("kfsmewsvspt-list-view");

            itemsSource = KFSMEWData.Instance.StateData.StatesFiltered;
            makeNoneElement = TestNoneKFSMController;
            makeItem = () => new KFSMEWSVSPListViewItem();
            bindItem = BindItem;
            reorderable = true;
            reorderMode = ListViewReorderMode.Animated;
            virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            itemIndexChanged += (oldIndex, newIndex) => KFSMEditorWindow.ChangeStateIndex(oldIndex, newIndex);
            selectionChanged += OnSelectedStateChange;

            OnControllerChange(KFSMEWData.Instance.Controller);

            KFSMEditorWindow.OnControllerChanged += OnControllerChange;
            KFSMEditorWindow.OnControllerChangedFinal += RefreshList;
            KFSMEditorWindow.OnSearchFieldValueChangedFinal += OnSearchFieldChanged;
            KFSMEditorWindow.OnSelectedStateDeselect += OnSelectedStateDeselect;
            KFSMEditorWindow.OnStateAddedFinal += RefreshList;
            KFSMEditorWindow.OnStateRemovedFinal += RefreshList;
            KFSMEditorWindow.OnStateIndexChangedFinal += RefreshList;

            RefreshList();
        }

        private void BindItem(VisualElement element, int index)
        {
            var stateContainer = KFSMEWData.Instance.StateData.StatesFiltered[index];
            var item = (KFSMEWSVSPListViewItem)element;

            item.Initialize(stateContainer);
        }
        private void OnSelectedStateChange(IEnumerable<object> enumerable)
        {
            foreach (var item in enumerable)
            {
                var container = (SKFSMStateContainer)item;
                KFSMEditorWindow.ChangeSelectedState(container);
                break;
            }
        }

        private void OnControllerChange(KFSMController controller)
        {
            makeNoneElement = controller == null ? TestNoneKFSMController : TestNoneStatesItems;
        }

        private void OnSearchFieldChanged()
        {
            reorderable = string.IsNullOrEmpty(KFSMEWData.Instance.StateData.SearchFieldText);
            reorderMode = reorderable ? ListViewReorderMode.Animated : ListViewReorderMode.Simple;

            RefreshList();
        }

        private void OnSelectedStateDeselect()
        {
            selectedIndex = -1;
        }

        private VisualElement TestNoneKFSMController() => MakeNoneElementBase("No KFSM Controller Selected");
        private VisualElement TestNoneStatesItems() => MakeNoneElementBase("No States Found");
        private VisualElement MakeNoneElementBase(string text)
        {
            // TODO: Move to uss and use addtoclasslist instead of inline styling
            var label = new Label(text)
            {
                style =
                {
                    paddingLeft = 5, paddingRight = 5, paddingTop = 10, paddingBottom = 5 ,
                    color = new Color(0.6f, 0.6f, 0.6f, 1f),
                    backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f),
                    fontSize = 12,
                    borderBottomWidth = 1,
                    borderBottomColor = new Color(0.1f, 0.1f, 0.1f, 1f),
                }
            };

            return label;
        }

        private void RefreshList()
        {
            RefreshItems();
            Rebuild();
        }
    }
}
