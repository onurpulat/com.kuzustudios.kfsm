using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVIPSVIPSVCIFVector3 : KFSMEWSVIPSVIPSVCItemField<Vector3Field, Vector3>
    {
        internal KFSMEWSVIPSVIPSVCIFVector3() : this("Vector3 Field", Vector3.zero, true, null) { }

        internal KFSMEWSVIPSVIPSVCIFVector3(string nameText, Vector3 value, bool enabled, EventCallback<ChangeEvent<Vector3>> changeEvent) : base(nameText, value, enabled, changeEvent) { }
    }
}
