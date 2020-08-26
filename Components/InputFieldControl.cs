using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using UnityEngine.UI;


namespace ObjectExplorer
{
    public class InputFieldControl : ControlBase<string> 
    {
       
        public InputField inputField; 
        
        public override void Refresh()  
        {
            if (inputField.isFocused) return;
            object value = currentTargetValue;
            if (value == null)
            {
                inputField.text = "";
            }
            else
            {
                inputField.text = value.ToString();
            }
        }

        public override void SetValue(string valueIn) 
        {
            bool succeeded = false;
            Type typeToConsider = hasSubMember ? currentSubMemberType : currentMemberType; 
            object setVal = null;
            if (typeToConsider == typeof(int))
            {
                succeeded = Int32.TryParse(valueIn, out int result);
                setVal = result;
            }
            else if (typeToConsider == typeof(float))
            {
                succeeded = float.TryParse(valueIn, out float result);
                setVal = result;
            }

            else if(typeToConsider == typeof(string))
            {
                succeeded = true;
                setVal = valueIn;
            }
            if(succeeded)
            {
                currentTargetValue = setVal;
            }
        }

        protected override void InitializeUIComponent()
        {
            if(!canEdit)
            {
                inputField.interactable = false;
                inputField.transform.Find("Text").GetComponent<Text>().color = TUNING.CONTROLS.NONEDITABLE.textColor;
            }
        }
    }
}
