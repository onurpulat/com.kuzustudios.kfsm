using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KuzuStudios.KFSM
{
    [CreateAssetMenu(fileName = "new KFSM State", menuName = "KuzuStudios/KFSM/State")]
    public class KFSMState : ScriptableObject
    {
        public virtual string StateName => name;
        
        public string DisplayName;
        public KFSMController Controller;

        public List<KFSMStateTransition> Transitions = new();

        public float StartTime;
        public float EndTime;

        public bool IsExitingState;
        public bool IsAnimationFinished;

        public float TestFloat;
        public bool TestBool;

        private readonly List<System.Action> _animationTriggers = new();

        public void OnInitialize(KFSMController controller)
        {
            Controller = controller;

            foreach (var transition in Transitions)
            {
                transition.Initialize(Controller);
            }
        }

        public void OnEnter()
        {
            StartTime = Time.time;

            IsAnimationFinished = false;
            IsExitingState = false;

            Enter();
        }

        public void OnExit()
        {
            EndTime = Time.time;

            IsExitingState = true;

            Exit();
        }

        public void OnUpdate()
        {
            CheckTransitions();

            if (IsExitingState) return;

            LogicUpdate();
        }

        public void OnFixedUpdate()
        {
            if (IsExitingState) return;

            FixedUpdate();
        }

        public void OnAnimationTrigger(int index)
        {
            if (index < 0 || index >= _animationTriggers.Count) return;
            _animationTriggers[index]?.Invoke();
        }

        public void OnAnimationFinishTrigger() { IsAnimationFinished = true; AnimationFinishTrigger(); }

        private void CheckTransitions()
        {
            foreach (var transition in Transitions)
            {
                if (transition.Evaluate())
                {
                    Controller.ChangeState(Controller.GetState(transition.TargetStateName));
                    return;
                }
            }
        }

        public void AddTransition(KFSMStateTransition transition)
        {
            if (transition != null && !Transitions.Contains(transition))
            {
                Transitions.Add(transition);
            }
        }

        public void RemoveTransition(KFSMStateTransition transition)
        {
            if (transition != null && Transitions.Contains(transition))
            {
                Transitions.Remove(transition);
            }
        }

        protected virtual void Enter() { }
        protected virtual void Exit() { }
        protected virtual void LogicUpdate() { }
        protected virtual void FixedUpdate() { }
        protected virtual void AnimationFinishTrigger() { }

        protected void AddAnimationTrigger(System.Action action) => _animationTriggers.Add(action);

        public KFSMState Clone()
        {
            var clone = ScriptableObject.Instantiate(this);

            var oldTransitions = clone.Transitions.ToList();
            clone.Transitions.Clear();
            foreach (var transition in oldTransitions)
            {
                var transitionClone = transition.Clone();
                clone.Transitions.Add(transitionClone);
            }

            return clone;
        }
    }
}