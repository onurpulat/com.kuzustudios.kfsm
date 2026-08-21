using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVIPSVIPSVCIFFloat : KFSMEWSVIPSVIPSVCItemField<FloatField, float>
    {
        internal KFSMEWSVIPSVIPSVCIFFloat() : base("Float Field", default, true, null) { }

        internal KFSMEWSVIPSVIPSVCIFFloat(string nameText, float value, bool enabled, EventCallback<ChangeEvent<float>> changeEvent) : base(nameText, value, enabled, changeEvent) { }
    }
}
