using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;

//maybe generic with two parameters -- property/field type and control type...
namespace ObjectExplorer
{
    public abstract class ControlBase<T> : MonoBehaviour
    {
        protected Component currentComponent;

        protected bool isProperty;
        protected bool isField;
        protected FieldInfo currentFieldInfo;
        protected PropertyInfo currentPropertyInfo;

        protected bool subIsProperty;
        protected bool subIsField;
        protected FieldInfo currentSubFieldInfo;
        protected PropertyInfo currentSubPropertyInfo;

        protected bool hasSubMember;
        protected Type currentMemberType;
        protected Type currentSubMemberType;

        protected bool canEdit;

        protected object currentComponentMember
        {
            get
            {
                if (isProperty)
                {
                    return currentPropertyInfo.GetValue(currentComponent, null);
                }
                else if (isField)
                {
                    return currentFieldInfo.GetValue(currentComponent);
                }
                else
                {
                    throw new InvalidOperationException("cannot get value where member is neither a field nor a property");
                }
            }
            set
            {
                Debug.Log("member set is being called");
                if (isProperty)
                {
                    Debug.Log("set called on property");
                    currentPropertyInfo.SetValue(currentComponent, value, null);
                }
                else if (isField)
                {
                    Debug.Log("set called on field");
                    currentFieldInfo.SetValue(currentComponent, value);
                }
                else
                {
                    throw new InvalidOperationException("cannot set value where member is neither a field nor a property");
                }
            }
        }
        protected object currentComponentSubMember
        {
            get
            {
                if (hasSubMember)
                {
                    if (subIsField)
                    {
                        return currentSubFieldInfo.GetValue(currentComponentMember);
                    }
                    else if (subIsProperty)
                    {
                        return currentSubPropertyInfo.GetValue(currentComponentMember, null);
                    }
                    else
                    {
                        throw new InvalidOperationException("cannot get value where submember is neither a field nor a property");
                    }
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Debug.Log("sub-member set is being called");
                if (currentMemberType.IsValueType)
                {
                    object newValueType = currentComponentMember;
                    if (subIsField)
                    {
                        currentSubFieldInfo.SetValue(newValueType, value);
                    }
                    else if (subIsProperty)
                    {
                        currentSubPropertyInfo.SetValue(newValueType, value, null);
                    }
                    else
                    {
                        throw new InvalidOperationException("cannot set value where sub-member is neither a field nor a property");
                    }
                    currentComponentMember = newValueType;
                }
                else
                {
                    if (subIsField)
                    {
                        currentSubFieldInfo.SetValue(currentComponentMember, value);
                    }
                    else if (subIsProperty)
                    {
                        currentSubPropertyInfo.SetValue(currentComponentMember, value, null);
                    }
                    else
                    {
                        throw new InvalidOperationException("cannot set value where sub-member is neither a field nor a property");
                    }
                }
            }
        }

        public void SetTarget(Component component, MemberInfo memberInfoIn, MemberInfo subMemberInfoIn = null)
        {
            currentComponent = component;
            hasSubMember = (subMemberInfoIn == null) ? false : true;

            InitializeMember(memberInfoIn);
            InitializeSubMember(subMemberInfoIn);
            InitializeCanEdit();

            InitializeUIComponent();

            Refresh();
        }

        protected void InitializeMember(MemberInfo memberInfoIn)
        {
            if (memberInfoIn.MemberType == MemberTypes.Field)
            {
                isField = true;
                isProperty = false;
                currentFieldInfo = (FieldInfo)memberInfoIn;
                currentPropertyInfo = null;
                currentMemberType = currentFieldInfo.FieldType;
            }
            else if (memberInfoIn.MemberType == MemberTypes.Property)
            {
                isField = false;
                isProperty = true;
                currentFieldInfo = null;
                currentPropertyInfo = (PropertyInfo)memberInfoIn;
                currentMemberType = currentPropertyInfo.PropertyType;
            }
            else
            {
                throw new ArgumentException("this control can only target a property or a field");
            }
        }

        protected void InitializeSubMember(MemberInfo subMemberInfoIn)
        {
            if (subMemberInfoIn == null)
            {
                currentSubFieldInfo = null;
                currentSubPropertyInfo = null;
                subIsProperty = false;
                subIsField = false;
                currentSubMemberType = null;
            }
            else if (subMemberInfoIn.MemberType == MemberTypes.Field)
            {
                subIsField = true;
                subIsProperty = false;
                currentSubFieldInfo = (FieldInfo)subMemberInfoIn;
                currentSubPropertyInfo = null;
                currentSubMemberType = currentSubFieldInfo.FieldType;
            }
            else if (subMemberInfoIn.MemberType == MemberTypes.Property)
            {
                subIsField = false;
                subIsProperty = true;
                currentSubFieldInfo = null;
                currentSubPropertyInfo = (PropertyInfo)subMemberInfoIn;
                currentSubMemberType = currentSubPropertyInfo.PropertyType;
            }
            else
            {
                throw new ArgumentException("this control can only target a property or a field");
            }
        }

        protected void InitializeCanEdit()
        {
            canEdit = false;
            if(!hasSubMember)
            {
                if(isProperty)
                {
                    if(currentPropertyInfo.GetSetMethod() != null)
                    {
                        canEdit = true;
                    }
                }
            }
            else //has sub member
            {
                if(subIsProperty)
                {
                    if(currentSubPropertyInfo.GetSetMethod() != null)
                    {
                        canEdit = true;
                    }
                }
                else if(currentMemberType.IsValueType)
                {
                    if(isProperty)
                    {
                        if(currentPropertyInfo.GetSetMethod() != null)
                        {
                            canEdit = true;
                        }
                    }
                }
            }
        }

        protected virtual void InitializeUIComponent() {}

        protected object GetCurrentValue()
        {
            object retVal;
            if (!hasSubMember)
            {
                retVal = currentComponentMember;
            }
            else
            {
                retVal = currentComponentSubMember;
            }
            return retVal;
        }

        public virtual void Refresh() { }

        public virtual void SetValue (T valueIn) { }
    }
}
