using UnityEngine;

namespace KuzuStudios.KFSM
{
    public class KFSM : MonoBehaviour
    {
        [SerializeField] private KFSMController m_contoller;

        #region Unity Callback Functions
        private void Awake() { if (m_contoller == null) return; m_contoller = m_contoller.Clone(); m_contoller.OnInitialize(); }
        private void Start() { if (m_contoller == null) return; m_contoller.OnStart(); }
        private void Update() { if (m_contoller == null) return; m_contoller.OnUpdate(); }
        private void FixedUpdate() { if (m_contoller == null) return; m_contoller.OnFixedUpdate(); }
        #endregion

        #region Animation Functions
        public void OnAnimationTrigger(int index) { if (m_contoller != null) m_contoller.OnAnimationTrigger(index); }
        public void OnAnimationFinishTrigger() { if (m_contoller != null) m_contoller.OnAnimationFinishTrigger(); }
        #endregion

        #region State Functions
        public void SetStartingState(KFSMState state) { if (m_contoller != null) m_contoller.SetStartingState(state); }
        public void SetStartingState(string stateName) { if (m_contoller != null) m_contoller.SetStartingState(stateName); }
        public void ChangeState(KFSMState state) { if (m_contoller != null) m_contoller.ChangeState(state); }
        public void ChangeState(string stateName) { if (m_contoller != null) m_contoller.ChangeState(stateName); }
        #endregion

        public KFSMController GetKFSMController() => m_contoller;
    }
}