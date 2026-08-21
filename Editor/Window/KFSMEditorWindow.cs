using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    public class KFSMEditorWindow : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        private static KFSMController _controller;
        public static KFSMController Controller
        {
            set
            {
                if (_controller == value) return;

                OnControllerChange?.Invoke(value);

                _controller = value;

                if (KFSMEWData.Instance.Controller != value)
                {
                    KFSMEWData.Instance.Controller = value;
                }

                OnControllerChanged?.Invoke(value);
                OnControllerChangedFinal?.Invoke();
            }
        }

        [MenuItem("KuzuStudios/KFSM/Editor Window")]
        public static void ShowExample()
        {
            KFSMEditorWindow wnd = GetWindow<KFSMEditorWindow>();
            wnd.titleContent = new GUIContent("KFSM");

            KFSMEWData.Instance.MakeSureCreated();
            Controller = KFSMEWData.Instance.Controller ? KFSMEWData.Instance.Controller : null;
        }

        private void OnDestroy()
        {
            Selection.selectionChanged -= SelectionChanged;
        }

        private void SelectionChanged()
        {
            if (Selection.activeObject is KFSMController controller)
            {
                Controller = controller;
            }
            else if (Selection.activeObject is GameObject selectedGO)
            {
                if (selectedGO.TryGetComponent(out KFSM kfsm))
                {
                    if (kfsm.GetKFSMController() != null)
                    {
                        Controller = kfsm.GetKFSMController();
                    }
                }
                else
                {
                    for (int i = 0; i < selectedGO.transform.childCount; i++)
                    {
                        var childGo = selectedGO.transform.GetChild(i);

                        if (childGo.TryGetComponent(out KFSM fsm))
                        {
                            if (fsm.GetKFSMController() != null)
                            {
                                Controller = fsm.GetKFSMController();
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void CreateGUI()
        {
            Selection.selectionChanged -= SelectionChanged;
            Selection.selectionChanged += SelectionChanged;

            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;
            root.style.flexGrow = 1;

            // Instantiate UXML
            VisualElement uxml = m_VisualTreeAsset.Instantiate();
            uxml.style.flexGrow = 1;
            root.Add(uxml);
        }

        #region Events
        // Controller Variable Change Event
        public static event Action<KFSMController> OnControllerChange;
        public static event Action<KFSMController> OnControllerChanged;
        public static event Action OnControllerChangedFinal;

        // KFSM Change Name Event
        public static event Action<string> OnKFSMNameChange;
        public static event Action<string> OnKFSMNameChanged;
        public static event Action OnKFSMNameChangedFinal;
        public static void ChangeKFSMName(string newName)
        {
            OnKFSMNameChange?.Invoke(newName);
            OnKFSMNameChanged?.Invoke(newName);
            OnKFSMNameChangedFinal?.Invoke();
        }

        // KFSM Search Field Value Change Event
        public static event Action<string> OnSearchFieldValueChange;
        public static event Action<string> OnSearchFieldValueChanged;
        public static event Action OnSearchFieldValueChangedFinal;
        public static void ChangeSearchFieldValue(string newValue)
        {
            OnSearchFieldValueChange?.Invoke(newValue);
            OnSearchFieldValueChanged?.Invoke(newValue);
            OnSearchFieldValueChangedFinal?.Invoke();
        }

        // KFSM State Change Name Event
        public static event Action<SKFSMStateContainer, string> OnStateDisplayNameChange;
        public static event Action<SKFSMStateContainer, string> OnStateDisplayNameChanged;
        public static event Action OnStateDisplayNameChangedFinal;
        public static void ChangeStateDisplayName(SKFSMStateContainer stateContainer, string newName)
        {
            OnStateDisplayNameChange?.Invoke(stateContainer, newName);
            OnStateDisplayNameChanged?.Invoke(stateContainer, newName);
            OnStateDisplayNameChangedFinal?.Invoke();
        }

        // KFSM Selected State Change Event
        public static event Action<SKFSMStateContainer> OnSelectedStateChange;
        public static event Action<SKFSMStateContainer> OnSelectedStateChanged;
        public static event Action OnSelectedStateChangedFinal;
        public static void ChangeSelectedState(SKFSMStateContainer stateContainer)
        {
            OnSelectedStateChange?.Invoke(stateContainer);
            OnSelectedStateChanged?.Invoke(stateContainer);
            OnSelectedStateChangedFinal?.Invoke();
        }

        // KFSM Deselect Selected State Event
        public static event Action OnSelectedStateDeselect;
        public static event Action OnSelectedStateDeselected;
        public static event Action OnSelectedStateDeselectedFinal;
        public static void DeselectSelectedState()
        {
            OnSelectedStateDeselect?.Invoke();
            OnSelectedStateDeselected?.Invoke();
            OnSelectedStateDeselectedFinal?.Invoke();
        }

        // KFSM State Index Change Event
        public static event Action<int, int> OnStateIndexChange;
        public static event Action<int, int> OnStateIndexChanged;
        public static event Action OnStateIndexChangedFinal;
        public static void ChangeStateIndex(int oldIndex, int newIndex)
        {
            OnStateIndexChange?.Invoke(oldIndex, newIndex);
            OnStateIndexChanged?.Invoke(oldIndex, newIndex);
            OnStateIndexChangedFinal?.Invoke();
        }

        // KFSM State Add Event
        public static event Action<SKFSMStateContainer> OnStateAdd;
        public static event Action<SKFSMStateContainer> OnStateAdded;
        public static event Action OnStateAddedFinal;
        public static void AddState(SKFSMStateContainer stateContainer)
        {
            OnStateAdd?.Invoke(stateContainer);
            OnStateAdded?.Invoke(stateContainer);
            OnStateAddedFinal?.Invoke();
        }

        // KFSM State Remove Event
        public static event Action<SKFSMStateContainer> OnStateRemove;
        public static event Action<SKFSMStateContainer> OnStateRemoved;
        public static event Action OnStateRemovedFinal;
        public static void RemoveState(SKFSMStateContainer stateContainer)
        {
            OnStateRemove?.Invoke(stateContainer);
            OnStateRemoved?.Invoke(stateContainer);
            OnStateRemovedFinal?.Invoke();
        }

        // KFSM State Transition Add Event
        public static event Action<SKFSMStateContainer> OnStateTransitionAdd;
        public static event Action<SKFSMStateContainer> OnStateTransitionAdded;
        public static event Action OnStateTransitionAddedFinal;
        public static void AddStateTranstion(SKFSMStateContainer transitionStateContainer)
        {
            OnStateTransitionAdd?.Invoke(transitionStateContainer);
            OnStateTransitionAdded?.Invoke(transitionStateContainer);
            OnStateTransitionAddedFinal?.Invoke();
        }

        // KFSM State Transition Popup Search Field Value Change Event
        public static event Action<string> OnPopupSearchFieldValueChange;
        public static event Action<string> OnPopupSearchFieldValueChanged;
        public static event Action OnPopupSearchFieldValueChangedFinal;
        public static void ChangePopupSearchFieldValue(string newValue)
        {
            OnPopupSearchFieldValueChange?.Invoke(newValue);
            OnPopupSearchFieldValueChanged?.Invoke(newValue);
            OnPopupSearchFieldValueChangedFinal?.Invoke();
        }

        // KFSM State Transition Popup Search Field Menu Change Event
        public static event Action<string> OnPopupSearchFieldMenuChange;
        public static event Action<string> OnPopupSearchFieldMenuChanged;
        public static event Action OnPopupSearchFieldMenuChangedFinal;
        public static void ChangePopupSearchFieldMenu(string newMenuItem)
        {
            OnPopupSearchFieldMenuChange?.Invoke(newMenuItem);
            OnPopupSearchFieldMenuChanged?.Invoke(newMenuItem);
            OnPopupSearchFieldMenuChangedFinal?.Invoke();
        }

        // KFSM State Transition On Selected State Change Event
        public static event Action<KFSMEWStateTransitionData.SKFSMStateTransitionContainer> OnSelectedStateTransitionChange;
        public static event Action<KFSMEWStateTransitionData.SKFSMStateTransitionContainer> OnSelectedStateTransitionChanged;
        public static event Action OnSelectedStateTransitionChangedFinal;
        public static void ChangeSelectedStateTransition(KFSMEWStateTransitionData.SKFSMStateTransitionContainer transitionContainer)
        {
            OnSelectedStateTransitionChange?.Invoke(transitionContainer);
            OnSelectedStateTransitionChanged?.Invoke(transitionContainer);
            OnSelectedStateTransitionChangedFinal?.Invoke();
        }

        // KFMS Deselect Selected State Transition 
        public static event Action OnSelectedStateTransitionDeselect;
        public static event Action OnSelectedStateTransitionDeselected;
        public static event Action OnSelectedStateTransitionDeselectedFinal;
        public static void DeselectSelectedStateTransition()
        {
            OnSelectedStateTransitionDeselect?.Invoke();
            OnSelectedStateTransitionDeselected?.Invoke();
            OnSelectedStateTransitionDeselectedFinal?.Invoke();
        }

        // KFSM State Change Event 
        public static event Action<SKFSMStateContainer, KFSMState> OnStateValueChange;
        public static event Action<SKFSMStateContainer, KFSMState> OnStateValueChanged;
        public static event Action OnStateValueChangedFinal;
        public static void ChangeStateValue(SKFSMStateContainer container, KFSMState newState)
        {
            OnStateValueChange?.Invoke(container, newState);
            OnStateValueChanged?.Invoke(container, newState);
            OnStateValueChangedFinal?.Invoke();
        }

        // KFSM Create State Transition Condition Event
        public static event Action OnStateTransitionConditionCreate;
        public static event Action OnStateTransitionConditionCreated;
        public static event Action OnStateTransitionConditionCreatedFinal;
        public static void CreateStateTransitionCondition()
        {
            OnStateTransitionConditionCreate?.Invoke();
            OnStateTransitionConditionCreated?.Invoke();
            OnStateTransitionConditionCreatedFinal?.Invoke();
        }

        public static event Action<KFSMCondition> OnStateTransitionConditionAdd;
        public static event Action<KFSMCondition> OnStateTransitionConditionAdded;
        public static event Action OnStateTransitionConditionAddedFinal;
        public static void StateConditionAdd(KFSMCondition condition)
        {
            OnStateTransitionConditionAdd?.Invoke(condition);
            OnStateTransitionConditionAdded?.Invoke(condition);
            OnStateTransitionConditionAddedFinal?.Invoke();
        }


        // KFSM Remove State Transition Condition Event
        public static event Action OnStateTransitionConditionRemove;
        public static event Action OnStateTransitionConditionRemoved;
        public static event Action OnStateTransitionConditionRemovedFinal;
        public static void RemoveStateTransitionCondition()
        {
            OnStateTransitionConditionRemove?.Invoke();
            OnStateTransitionConditionRemoved?.Invoke();
            OnStateTransitionConditionRemovedFinal?.Invoke();
        }

        // KFSM Selected Condition Index Change Event
        public static event Action<int> OnSelectedConditionIndexChange;
        public static event Action<int> OnSelectedConditionIndexChanged;
        public static event Action OnSelectedConditionIndexChangedFinal;
        public static void ChangeSelectedConditionIndex(int newIndex)
        {
            OnSelectedConditionIndexChange?.Invoke(newIndex);
            OnSelectedConditionIndexChanged?.Invoke(newIndex);
            OnSelectedConditionIndexChangedFinal?.Invoke();
        }
        #endregion
    }
}