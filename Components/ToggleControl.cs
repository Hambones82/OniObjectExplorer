using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace ObjectExplorer
{
    public class ToggleControl : ControlBase<bool>
    {
        public Toggle toggle;
        public ToggleHelper toggleHelper;

        public override void Refresh()
        {
            //object value = GetCurrentValue();
            toggleHelper.SetIsOn((bool)currentTargetValue);
        }

        public override void SetValue(bool valueIn) => currentTargetValue = valueIn;
        
        protected override void InitializeUIComponent()
        {
            if (!canEdit)
            {
                toggle.interactable = false;
                toggle.transform.Find("Background/Checkmark").GetComponent<Image>().color = TUNING.CONTROLS.NONEDITABLE.textColor;
            }
        }
    }
}
