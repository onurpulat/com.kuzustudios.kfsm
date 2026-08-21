using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KuzuStudios.KFSM
{
    public class KFSMStateTransition : ScriptableObject
    {
        public string TargetStateName;
        public List<KFSMCondition> Conditions = new();

        public KFSMController Controller;

        public void Initialize(KFSMController controller)
        {
            Controller = controller;

            foreach (var condition in Conditions)
            {
                condition.Initialize(Controller);
            }
        }

        public void AddCondition(KFSMCondition condition)
        {
            if (condition != null && !Conditions.Contains(condition))
            {
                Conditions.Add(condition);
            }
        }

        public bool Evaluate()
        {
            foreach (var condition in Conditions)
            {
                if (!condition.Evaluate()) return false;
            }

            return true;
        }

        public KFSMStateTransition Clone()
        {
            var clone = ScriptableObject.Instantiate(this);

            var oldConditions = clone.Conditions.ToList();
            clone.Conditions.Clear();
            foreach (var condition in oldConditions)
            {
                var conditionClone = condition.Clone();
                clone.Conditions.Add(conditionClone);
            }

            return clone;
        }
    }
}
