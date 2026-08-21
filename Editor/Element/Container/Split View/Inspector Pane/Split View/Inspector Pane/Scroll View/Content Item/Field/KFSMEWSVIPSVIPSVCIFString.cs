using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVIPSVIPSVCIFString : KFSMEWSVIPSVIPSVCItemField<TextField, string>
    {
        internal KFSMEWSVIPSVIPSVCIFString() : this("String Field", string.Empty, true, null) { }

        internal KFSMEWSVIPSVIPSVCIFString(string nameText, string value, bool enabled, EventCallback<ChangeEvent<string>> changeEvent) : base(nameText, value, enabled, changeEvent) { }
    }
}
