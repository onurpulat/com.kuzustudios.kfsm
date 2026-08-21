using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace KuzuStudios.KFSM.Editor
{
    internal partial class KFSMEWSVIPSVIPSVCIFColor : KFSMEWSVIPSVIPSVCItemField<ColorField, Color>
    {
        internal KFSMEWSVIPSVIPSVCIFColor() : this("Color Field", default, true, null) { }

        internal KFSMEWSVIPSVIPSVCIFColor(string nameText, Color value, bool enabled, EventCallback<ChangeEvent<Color>> changeEvent) : base(nameText, value, enabled, changeEvent) { }
    }
}
