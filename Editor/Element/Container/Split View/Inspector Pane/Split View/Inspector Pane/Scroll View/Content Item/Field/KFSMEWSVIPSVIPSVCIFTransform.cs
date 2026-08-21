using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVIPSVIPSVCIFTransform : KFSMEWSVIPSVIPSVCItemField<ObjectField, Object>
    {
        internal KFSMEWSVIPSVIPSVCIFTransform() : this("Transform Field", null, true, null) { }

        internal KFSMEWSVIPSVIPSVCIFTransform(string nameText, Transform value, bool enabled, EventCallback<ChangeEvent<Object>> changeEvent) : base(nameText, value, enabled, changeEvent)
        {
            Field.dataSourceType = typeof(Transform);
            Field.objectType = typeof(Transform);
            Field.allowSceneObjects = true;
        }
    }
}
