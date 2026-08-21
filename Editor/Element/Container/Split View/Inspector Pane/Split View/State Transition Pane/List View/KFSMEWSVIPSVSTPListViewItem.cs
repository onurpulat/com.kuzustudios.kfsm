using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVIPSVSTPListViewItem : VisualElement
    {
        internal Label NameLabel { get; private set; }

        private KFSMEWStateTransitionData.SKFSMStateTransitionContainer _container;

        public KFSMEWSVIPSVSTPListViewItem()
        {
            AddToClassList("kfsmewsvipsvstp-list-view-item");
        }

        internal void Initialize(KFSMEWStateTransitionData.SKFSMStateTransitionContainer stateTransitionContainer)
        {
            Clear();

            NameLabel = new();

            _container = stateTransitionContainer;

            NameLabel.text = stateTransitionContainer.DisplayName;

            Add(NameLabel);
        }
    }
}
