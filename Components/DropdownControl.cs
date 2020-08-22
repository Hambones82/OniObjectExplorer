using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;

namespace ObjectExplorer
{
    public class DropdownControl : ControlBase<int>
    {
        public Dropdown dropdown;
        public DropdownHelper dropdownHelper;

        private Dictionary<int, object> enumValueIndices = new Dictionary<int, object>();

        public void ClearState()
        {
            dropdown.ClearOptions();
            enumValueIndices.Clear();
        }

        //probably still need to set all the pulldown values even if not editable...
        protected override void InitializeUIComponent()
        {
            if (!canEdit)
            {
                dropdown.interactable = false;
                dropdown.transform.Find("Label").GetComponent<Text>().color = TUNING.CONTROLS.NONEDITABLE.textColor;
                dropdown.transform.Find("Arrow").GetComponent<Image>().color = TUNING.CONTROLS.NONEDITABLE.textColor;
            }
            
            object[] enumValues = Enum.GetValues(currentTargetType).Cast<object>().ToArray();
            for (int i = 0; i < enumValues.Length; i++)
            {
                enumValueIndices.Add(i, enumValues.GetValue(i));
            }
            dropdown.AddOptions(enumValues.Select<object, string>((o, s) => o.ToString()).ToList<string>());
            dropdown.RefreshShownValue();
            
        }

        public override void Refresh()
        {
            object value = currentTargetValue;//this is going to be probably an int... possibly it will be expressed as an enum
            
            int valueToSet = enumValueIndices.First((KeyValuePair<int, object> kvp) => object.Equals(kvp.Value, value)).Key;
            
            dropdownHelper.SetValue(valueToSet);
            dropdown.RefreshShownValue();
        }

        public override void SetValue(int valueIn)
        {
            Debug.Log("calling set value for dropdown");
            currentTargetValue = enumValueIndices[valueIn];
        }

    }
}
