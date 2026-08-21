using KuzuStudios.Kutils;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal class KFSMEWSVSPListViewItem : VisualElement
    {
        internal Label NameLabel { get; private set; }
        internal TextField NameTextField { get; private set; }

        private SKFSMStateContainer _stateContainer;

        internal KFSMEWSVSPListViewItem()
        {
            AddToClassList("kfsmewsvspt-list-view-item");
        }

        internal void Initialize(SKFSMStateContainer stateContainer)
        {
            Clear();

            _stateContainer = stateContainer;

            NameLabel = new();
            NameLabel.AddToClassList("kfsmewsvspt-list-view-item-label");
            NameLabel.text = _stateContainer.DisplayName;
            if (string.IsNullOrEmpty(KFSMEWData.Instance.StateData.SearchFieldText))
            {
                NameLabel.UnregisterCallback<PointerDownEvent>(NameLabelPointerDownEvent, TrickleDown.TrickleDown);
                NameLabel.RegisterCallback<PointerDownEvent>(NameLabelPointerDownEvent, TrickleDown.TrickleDown);
            }

            NameTextField = new();
            NameTextField.AddToClassList("kfsmewsvspt-list-view-item-text-field");

            Add(NameLabel);

            this.AddManipulator(new ContextualMenuManipulator(OnContextualMenuManipulator));
        }

        private void NameLabelPointerDownEvent(PointerDownEvent evt)
        {
            if (evt.clickCount == 2 && evt.button == 0)
            {
                NameTextField.value = NameLabel.text;
                Insert(IndexOf(NameLabel), NameTextField);
                Remove(NameLabel);
                NameTextField.Focus();

                NameTextField.RegisterCallback<FocusOutEvent>(NameTextFieldFocusOutEvent, TrickleDown.TrickleDown);
                NameTextField.RegisterCallback<KeyDownEvent>(NameTextFieldKeyDownEvent, TrickleDown.TrickleDown);
            }
        }

        private void NameTextFieldFocusOutEvent(FocusOutEvent evt)
        {
            if (Contains(NameTextField))
            {
                if (!string.IsNullOrEmpty(NameTextField.value) && NameTextField.value != NameLabel.text)
                {
                    var checkList = KFSMEWData.Instance.StateData.States.ConvertAll(s => s.DisplayName).ToList();
                    var uniqueName = StringUtils.GetUniqueName(NameTextField.value, checkList);

                    KFSMEditorWindow.ChangeStateDisplayName(_stateContainer, uniqueName);

                    var tmpCon = _stateContainer;
                    tmpCon.DisplayName = uniqueName;

                    _stateContainer = tmpCon;

                    NameLabel.text = _stateContainer.DisplayName;
                }


                Insert(IndexOf(NameTextField), NameLabel);
                Remove(NameTextField);
                NameLabel.style.display = DisplayStyle.Flex;
            }

            NameTextField.UnregisterCallback<FocusOutEvent>(NameTextFieldFocusOutEvent);
            NameTextField.UnregisterCallback<KeyDownEvent>(NameTextFieldKeyDownEvent);
        }

        private void NameTextFieldKeyDownEvent(KeyDownEvent evt)
        {
            if (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter)
            {
                NameTextFieldFocusOutEvent(null);
            }
            else if (evt.keyCode == KeyCode.Escape)
            {
                NameTextField.style.display = DisplayStyle.None;
                NameLabel.style.display = DisplayStyle.Flex;
            }
        }

        private void OnContextualMenuManipulator(ContextualMenuPopulateEvent @evt)
        {
            @evt.menu.AppendAction("Remove State", action =>
            {
                if (EditorUtility.DisplayDialog("Remove State", $"Are you sure you want to remove the state '{_stateContainer.DisplayName}'?", "Remove", "Cancel"))
                {
                    KFSMEditorWindow.RemoveState(_stateContainer);
                }
            });
        }
    }
}
