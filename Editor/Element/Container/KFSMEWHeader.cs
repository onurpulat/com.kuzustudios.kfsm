using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWHeader : VisualElement
    {
        private const string _default_title = "KFSM";

        internal Label TitleLabel { get; private set; }
        internal TextField TitleTextField { get; private set; }

        internal KFSMEWHeader()
        {
            AddToClassList("kfsmew-header");

            TitleLabel = new();
            TitleLabel.AddToClassList("kfsmew-header-title-label");

            TitleTextField = new();
            TitleTextField.AddToClassList("kfsmew-header-title-text-field");

            Add(TitleLabel);

            OnControllerChange(KFSMEWData.Instance.Controller);
            KFSMEditorWindow.OnControllerChange += OnControllerChange;
        }

        private void OnControllerChange(KFSMController controller)
        {
            if (controller == null)
            {
                TitleLabel.UnregisterCallback<PointerDownEvent>(TitleLabelPointerDownEvent, TrickleDown.TrickleDown);
                TitleLabel.text = _default_title;
            }
            else
            {
                TitleLabel.UnregisterCallback<PointerDownEvent>(TitleLabelPointerDownEvent, TrickleDown.TrickleDown);
                TitleLabel.RegisterCallback<PointerDownEvent>(TitleLabelPointerDownEvent, TrickleDown.TrickleDown);
                TitleLabel.text = string.IsNullOrEmpty(controller.name) ? _default_title : controller.name;
            }
        }

        private void TitleLabelPointerDownEvent(PointerDownEvent evt)
        {
            if (evt.clickCount == 2 && evt.button == 0)
            {
                TitleTextField.value = TitleLabel.text;

                Insert(IndexOf(TitleLabel), TitleTextField);
                Remove(TitleLabel);

                TitleTextField.Focus();

                TitleTextField.RegisterCallback<FocusOutEvent>(TitleTextFieldFocusOutEvent, TrickleDown.TrickleDown);
                TitleTextField.RegisterCallback<KeyDownEvent>(TitleTextFieldKeyDownEvent, TrickleDown.TrickleDown);
            }
        }

        private void TitleTextFieldFocusOutEvent(FocusOutEvent evt)
        {
            if (Contains(TitleTextField))
            {
                if (!string.IsNullOrEmpty(TitleTextField.value) && TitleTextField.value != TitleLabel.text)
                {
                    var newName = TitleTextField.value;
                    TitleLabel.text = newName;
                    KFSMEditorWindow.ChangeKFSMName(newName);
                }

                Insert(IndexOf(TitleTextField), TitleLabel);
                Remove(TitleTextField);
                TitleLabel.style.display = DisplayStyle.Flex;
            }

            TitleTextField.UnregisterCallback<FocusOutEvent>(TitleTextFieldFocusOutEvent);
            TitleTextField.UnregisterCallback<KeyDownEvent>(TitleTextFieldKeyDownEvent);
        }

        private void TitleTextFieldKeyDownEvent(KeyDownEvent evt)
        {
            if (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter)
            {
                TitleTextFieldFocusOutEvent(null);
            }
            else if (evt.keyCode == KeyCode.Escape)
            {
                TitleTextField.style.display = DisplayStyle.None;
                TitleLabel.style.display = DisplayStyle.Flex;
            }
        }
    }
}
