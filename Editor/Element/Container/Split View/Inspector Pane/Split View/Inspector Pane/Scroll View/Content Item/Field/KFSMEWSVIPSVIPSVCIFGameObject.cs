using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVIPSVIPSVCIFGameObject : KFSMEWSVIPSVIPSVCItemField<ObjectField, Object>
    {
        internal KFSMEWSVIPSVIPSVCIFGameObject() : this("Game Object Field", null, true, null) { }

        internal KFSMEWSVIPSVIPSVCIFGameObject(string nameText, GameObject value, bool enabled, EventCallback<ChangeEvent<Object>> changeEvent) : base(nameText, value, enabled, changeEvent)
        {
            Field.dataSourceType = typeof(GameObject);
            Field.objectType = typeof(GameObject);
            Field.allowSceneObjects = true;
        }
    }
}
