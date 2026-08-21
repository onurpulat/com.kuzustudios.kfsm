using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVIPSVIPSVCIFBool : KFSMEWSVIPSVIPSVCItemField<Toggle, bool>
    {
        internal KFSMEWSVIPSVIPSVCIFBool() : this("Bool Field", default, true, null) { }

        public KFSMEWSVIPSVIPSVCIFBool(string nameText, bool value, bool enabled, EventCallback<ChangeEvent<bool>> changeEvent) : base(nameText, value, enabled, changeEvent) { }
    }
}
