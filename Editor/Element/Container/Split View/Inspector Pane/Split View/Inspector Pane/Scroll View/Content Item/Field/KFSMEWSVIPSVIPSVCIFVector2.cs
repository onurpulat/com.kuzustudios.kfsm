using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVIPSVIPSVCIFVector2 : KFSMEWSVIPSVIPSVCItemField<Vector2Field, Vector2>
    {
        internal KFSMEWSVIPSVIPSVCIFVector2() : this("Vector2 Field", Vector2.zero, true, null) { }

        internal KFSMEWSVIPSVIPSVCIFVector2(string nameText, Vector2 value, bool enabled, EventCallback<ChangeEvent<Vector2>> changeEvent) : base(nameText, value, enabled, changeEvent) { }
    }
}
