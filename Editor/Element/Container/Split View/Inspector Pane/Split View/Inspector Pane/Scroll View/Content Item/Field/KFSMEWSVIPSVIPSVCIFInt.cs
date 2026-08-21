using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVIPSVIPSVCIFInt : KFSMEWSVIPSVIPSVCItemField<IntegerField, int>
    {
        internal KFSMEWSVIPSVIPSVCIFInt() : this("Integer Field", default, true, null) { }

        internal KFSMEWSVIPSVIPSVCIFInt(string nameText, int value, bool enabled, EventCallback<ChangeEvent<int>> changeEvent) : base(nameText, value, enabled, changeEvent) { }
    }
}
