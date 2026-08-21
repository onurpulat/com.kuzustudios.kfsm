using KuzuStudios.Kutils;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVIPSVIPSVCItemField<TField, TValue> : KFSMEWSVIPSVIPSVCItemBase
        where TField : BaseField<TValue>, new()
    {
        internal TField Field { get; private set; }
        internal TValue Value { get; private set; }

        internal KFSMEWSVIPSVIPSVCItemField() : this("Item Field", default, true, null) { }

        internal KFSMEWSVIPSVIPSVCItemField(string nameText, TValue value, bool enabled, EventCallback<ChangeEvent<TValue>> changeEvent) : base(nameText)
        {
            AddToClassList("kfsmewsvipsvc-item-field");

            Field = new();
            if (changeEvent != null) Field.RegisterValueChangedCallback(changeEvent);
            Field.value = value;
            Field.AddToClassList("kfsmewsvipsvc-item-field-value");
            NameLabel.AddToClassList("kfsmewsvipsvc-item-field-label");

            if (!enabled) Field.SetEnabled(false);

            Add(Field);
        }
    }
}
