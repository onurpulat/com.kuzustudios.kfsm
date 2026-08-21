using UnityEngine;

namespace KuzuStudios.KFSM
{
    public class KFSMCondition : ScriptableObject
    {
        public const string THRESHOLD_PARAMETER_NAME = "None";

        public string Parameter1Name;
        public EKFSMOperationType OperationType;
        public string Parameter2Name;
        public float Parameter2Threshold;

        public KFSMController Controller;

        public void Initialize(KFSMController controller)
        {
            Controller = controller;
        }

        public bool Evaluate()
        {
            float valueParam1 = System.Convert.ToSingle(Controller.Blackboard.GetValue(Parameter1Name));
            float valueParam2 = Parameter2Name != THRESHOLD_PARAMETER_NAME ? 
                System.Convert.ToSingle(Controller.Blackboard.GetValue(Parameter2Name)) : 
                Parameter2Threshold;

            return OperationType switch
            {
                EKFSMOperationType.Equal => valueParam1 == valueParam2,
                EKFSMOperationType.NotEqual => valueParam1 != valueParam2,
                EKFSMOperationType.GreaterThan => valueParam1 > valueParam2,
                EKFSMOperationType.LessThan => valueParam1 < valueParam2,
                EKFSMOperationType.GreaterThanOrEqual => valueParam1 > valueParam2,
                EKFSMOperationType.LessThanOrEqual => valueParam1 <= valueParam2,
                _ => false
            };
        }

        public KFSMCondition Clone()
        {
            var clone = ScriptableObject.Instantiate(this);

            return clone;
        }
    }
}