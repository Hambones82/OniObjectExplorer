using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using UnityEngine.UI;

//is it possibly not working for sub-properties?
namespace ObjectExplorer
{
    public class InputFieldControl : MonoBehaviour // : InspectorControl : Monobehaviour???  maybe not necessary?
    {
        private Component currentComponent;
        bool isProperty;
        bool isField;
        private FieldInfo currentFieldInfo;
        private PropertyInfo currentPropertyInfo;
        private FieldInfo currentSubFieldInfo;
        bool hasSubField;
        private Type currentMemberType; 
        private Type currentSubMemberType;

        //caching these doesn't work for value types...  i will have to switch them to properties i guess
        private object currentComponentMember
        {
            get
            {
                if (isProperty)
                {
                    return currentPropertyInfo.GetValue(currentComponent, null);
                }
                else 
                {
                    return currentFieldInfo.GetValue(currentComponent);
                }
            }
        }
        private object currentComponentSubMember
        {
            get
            {
                if (hasSubField)
                {
                    return currentSubFieldInfo.GetValue(currentComponentMember);
                }
                else
                {
                    return null;
                }
            }
            
        }

        public InputField inputField;

        //called by default inspector generator
        //
        public void SetTarget(Component component, FieldInfo fieldInfoIn, FieldInfo subFieldInfoIn = null)
        {
            isField = true;
            isProperty = false;
            Initialize(component, null, fieldInfoIn, subFieldInfoIn, fieldInfoIn.FieldType, subFieldInfoIn?.FieldType);
        }

        public void SetTarget(Component component, PropertyInfo propertyInfoIn, FieldInfo subFieldInfoIn = null)
        {
            isField = false;
            isProperty = true;
            Initialize(component, propertyInfoIn, null, subFieldInfoIn, propertyInfoIn.PropertyType, subFieldInfoIn?.FieldType);
        }

        private void Initialize(Component component, PropertyInfo propertyInfoIn, FieldInfo fieldInfoIn, 
            FieldInfo subFieldInfoIn, Type currentTypeIn, Type currentSubTypeIn)
        {
            currentComponent = component; //fine
            currentFieldInfo = fieldInfoIn;
            currentPropertyInfo = propertyInfoIn;
            currentSubFieldInfo = subFieldInfoIn;
            currentMemberType = currentTypeIn;
            currentSubMemberType = currentSubTypeIn;
            hasSubField = subFieldInfoIn == null ? false : true;

            Refresh();
        }
        
        public void Refresh() 
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
            //update the input field's text with the current value
        }

        private object GetCurrentValue()
        {
            //null check
            object retVal;
            if (!hasSubField)
            {
                retVal = currentComponentMember;
            }
            else
            {
                retVal = currentComponentSubMember;
            }
            return retVal;
        }
        

        public bool SetValue(string valueIn) //returns whether set value succeeds - can fail if the string cannot be parsed into 
                                             //the object type
        {
            //check if has a setter?
            Debug.Log($"calling setvalue, valueIn is {valueIn}");
            bool succeeded = false;
            Type typeToConsider = hasSubField ? currentSubMemberType : currentMemberType; 
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
                if (hasSubField) //check...  whether the type is structure...
                {
                    if(currentMemberType.IsValueType)
                    {
                        object newValueType = currentComponentMember;
                        currentSubFieldInfo.SetValue(newValueType, setVal);
                        succeeded = SetComponentOrFieldValue(newValueType);
                    }
                    else
                    {
                        currentSubFieldInfo.SetValue(currentComponentSubMember, setVal);
                    }
                }
                else //check if property or field
                {
                    succeeded = SetComponentOrFieldValue(setVal);
                }
            }
            //set the value of the underlying variable based on the string received from the input field
            return succeeded;
        }

        private bool SetComponentOrFieldValue(object value)
        {
            if(isProperty)
            {
                if (currentPropertyInfo.GetSetMethod() == null) return false;
                currentPropertyInfo.SetValue(currentComponent, value, null);
            }
            else
            {
                currentFieldInfo.SetValue(currentComponent, value);
            }
            return true;
        }        
    }
}
