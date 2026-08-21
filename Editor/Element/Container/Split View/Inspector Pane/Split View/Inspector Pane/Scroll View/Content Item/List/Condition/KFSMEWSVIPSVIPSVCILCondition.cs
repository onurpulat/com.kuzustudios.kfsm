using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVIPSVIPSVCILCondition : KFSMEWSVIPSVIPSVCItemList
    {
        internal KFSMEWSVIPSVIPSVCILCondition() : this("Conditionss") { }
        internal KFSMEWSVIPSVIPSVCILCondition(string name) : base(name)
        {
            List.itemsSource = KFSMEWData.Instance.ConditionData.Conditions;
            List.bindItem = BindItem;
            List.makeItem = () => new KFSMEWSVIPSVIPSVCILConditionItem();
            List.selectionType = SelectionType.Single;
            List.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            List.selectedIndicesChanged += OnSelectedIndicesChanged;

            AddButton.clicked += OnButtonAdd;
            RemoveButton.clicked += OnButtonRemove;

            KFSMEditorWindow.OnStateTransitionConditionAddedFinal += RefreshList;
            KFSMEditorWindow.OnStateTransitionConditionRemovedFinal += RefreshList;
            KFSMEditorWindow.OnSelectedStateTransitionChangedFinal += DeselectList;
            RefreshList();
        }

        private void BindItem(VisualElement element, int index)
        {
            var item = (KFSMEWSVIPSVIPSVCILConditionItem)element;
            var condition = KFSMEWData.Instance.ConditionData.Conditions[index];

            item.Initialize(condition);
        }

        private void OnSelectedIndicesChanged(IEnumerable<int> enumerable)
        {
            foreach (var index in enumerable)
            {
                KFSMEditorWindow.ChangeSelectedConditionIndex(index);
                break;
            }
        }

        private void OnButtonAdd()
        {
            KFSMEditorWindow.CreateStateTransitionCondition();
        }

        private void OnButtonRemove()
        {
            KFSMEditorWindow.RemoveStateTransitionCondition();
        }

        private void DeselectList()
        {
            List.selectedIndex = -1;
        }
    }
}
