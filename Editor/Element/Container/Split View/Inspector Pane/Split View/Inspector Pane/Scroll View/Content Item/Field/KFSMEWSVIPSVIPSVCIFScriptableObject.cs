using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVIPSVIPSVCIFScriptableObject<T> : KFSMEWSVIPSVIPSVCItemField<ObjectField, Object>
        where T : ScriptableObject
    {
        internal KFSMEWSVIPSVIPSVCIFScriptableObject() : this("Scriptable Object Field", null, true, null) { }

        internal KFSMEWSVIPSVIPSVCIFScriptableObject(string nameText, T value, bool enabled, EventCallback<ChangeEvent<Object>> changeEvent) : base(nameText, value, enabled, changeEvent)
        {
            Field.dataSourceType = typeof(T);
            Field.objectType = typeof(T);
            Field.allowSceneObjects = false;
        }
    }
}
