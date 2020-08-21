using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using UnityEngine.UI;

//is it possibly not working for sub-properties?
//make the abstract class generic.  then the sub-classes will derive from generic with specified type..
namespace ObjectExplorer
{
    public class InputFieldControl : ControlBase<string> // : InspectorControl : Monobehaviour???  maybe not necessary?
    {
       
        public InputField inputField; //in abstract class, selectable?  or just nothing?  probably nothing, define here.
        
        public override void Refresh()  
        {
            object value = GetCurrentValue();
            if (value == null)
            {
                inputField.text = "";
            }
            else
            {
                Debug.Log($"We are trying to set a particular value in refresh: {value.ToString()}");
                inputField.text = value.ToString();
            }
        }

        public override void SetValue(string valueIn) 
        {
            Debug.Log($"calling setvalue, valueIn is {valueIn}");
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
                Debug.Log($"succeeded in parsing input {valueIn}.");
                Debug.Log($"parsed value is {setVal.ToString()}");
                Debug.Log($"type of parsed value is {setVal.GetType()}");
                if (hasSubMember) 
                {
                    currentComponentSubMember = setVal;
                }
                else 
                {
                    currentComponentMember = setVal;
                }
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
