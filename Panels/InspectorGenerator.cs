using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using UnityEngine.UI;
using UnityEngine.Events;

namespace ObjectExplorer
{
    
    public class InspectorGenerator
    {
        private GameObject timerObject;
        private Timer timer;

        private List<ControlBase> currentContentObjects = new List<ControlBase>();

        private static LoadedAssets.AssetEnums labelType = LoadedAssets.AssetEnums.inspectorlabel;
        private static LoadedAssets.AssetEnums inputFieldType = LoadedAssets.AssetEnums.inspectorinputfield;
        private static LoadedAssets.AssetEnums toggleType = LoadedAssets.AssetEnums.inspectortoggle;
        private static LoadedAssets.AssetEnums dropdownType = LoadedAssets.AssetEnums.inspectordropdown;

        private UIObjectPool labelPool;
        private UIObjectPool fieldPool;
        private UIObjectPool togglePool;
        private UIObjectPool dropdownPool;

        public InspectorGenerator(ExplorerManager eManager)
        {
            labelPool = new UIObjectPool(new LabelCreator(labelType), new LabelRecycler());
            fieldPool = new UIObjectPool(new InputFieldCreator(inputFieldType), new InputFieldRecycler());
            togglePool = new UIObjectPool(new ToggleCreator(toggleType, eManager), new ToggleRecycler());
            dropdownPool = new UIObjectPool(new DropdownCreator(dropdownType), new DropdownRecycler());

            timerObject = new GameObject("timer");
            timer = timerObject.AddComponent<Timer>();
            timer.period = TUNING.CONTROLS.REFRESH_RATE;
            timer.timerCallback += RefreshInspectors;
            timerObject.SetActive(true);
        }
        
        public IEnumerable<List<GameObject>> GetComponentControls(Component c)
        {
            currentContentObjects.Clear();
            Type cType = c.GetType();
            if(InspectorSpecifications.ComponentSpecifications.ContainsKey(cType))
            {
                string[] memberNames;
                InspectorSpecifications.ComponentSpecifications.TryGetValue(cType, out memberNames);
                for(int i = 0; i < memberNames.Length; i++)
                {
                    PropertyInfo property = cType.GetProperty(memberNames[i]);
                    FieldInfo field = cType.GetField(memberNames[i]);
                    if(property != null)
                    {
                        foreach (List<GameObject> goList in GetMemberControls(c, property))
                        {
                            yield return goList;
                        }
                    }
                    else if(field != null)
                    {
                        foreach(List<GameObject> goList in GetMemberControls(c, field))
                        {
                            yield return goList;
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("member name in specification does not exist in component");
                    }
                }
            }
            else
            {
                PropertyInfo[] properties = cType.GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    Type pType = properties[i].PropertyType;
                    if (InspectorSpecifications.IsPermittedType(pType))
                    {
                        foreach (List<GameObject> goList in GetMemberControls(c, properties[i]))
                        {
                            yield return goList;
                        }
                    }
                }
                FieldInfo[] fields = cType.GetFields();
                for (int i = 0; i < fields.Length; i++)
                {
                    Type fType = fields[i].FieldType;
                    if (InspectorSpecifications.IsPermittedType(fType))
                    {
                        foreach (List<GameObject> goList in GetMemberControls(c, fields[i]))
                        {
                            yield return goList;
                        }
                    }
                }
            }
            yield break;
        }

        public IEnumerable<List<GameObject>> GetMemberControls(Component c, MemberInfo memberInfo)
        {
            Type memberType;

            if (memberInfo.MemberType == MemberTypes.Property)
            {
                if (!((PropertyInfo)memberInfo).CanRead) yield break;
                memberType = ((PropertyInfo)memberInfo).PropertyType;
            }
            else if (memberInfo.MemberType == MemberTypes.Field)
            {
                memberType = ((FieldInfo)memberInfo).FieldType;
            }
            else throw new InvalidOperationException("member must be a field or a property");
            
            if(InspectorSpecifications.IsPrimitiveType(memberType))
            {
                yield return GetObjectList(c, memberInfo, null, memberType);
            }
            else
            {
                if(InspectorSpecifications.MemberSpecifications.ContainsKey(memberType))
                {
                    string[] membersToDisplay;
                    InspectorSpecifications.MemberSpecifications.TryGetValue(memberType, out membersToDisplay);
                    for (int i = 0; i < membersToDisplay.Length; i++)
                    {
                        FieldInfo field = memberType.GetField(membersToDisplay[i]);
                        PropertyInfo property = memberType.GetProperty(membersToDisplay[i]);
                        if(field != null)
                        {
                            yield return GetObjectList(c, memberInfo, field, field.FieldType, "." + field.Name);
                        }
                        else if(property != null)
                        {
                            yield return GetObjectList(c, memberInfo, property, property.PropertyType, "." + property.Name);
                        }
                        else
                        {
                            throw new InvalidOperationException("the provided member specification includes an element not found in the component for the control");
                        }
                    }
                }
                else
                {
                    FieldInfo[] fields = memberType.GetFields();
                    foreach (FieldInfo field in fields)
                    {
                        if (InspectorSpecifications.IsPermittedBasicField(field))
                        {
                            yield return GetObjectList(c, memberInfo, field, field.FieldType, "." + field.Name);
                        }
                    }

                    PropertyInfo[] properties = memberType.GetProperties();
                    foreach (PropertyInfo property in properties)
                    {

                        if(InspectorSpecifications.IsPermittedBasicProperty(property))
                        {
                            yield return GetObjectList(c, memberInfo, property, property.PropertyType, "." + property.Name);
                        }
                    }
                }  
            }
            yield break;
        }

        private List<GameObject> GetObjectList(Component c, MemberInfo memberInfo, MemberInfo subMemberInfo, Type targetType, string suffix = "")
        {
            List<GameObject> returnObject = new List<GameObject>();
            returnObject.Add(GetLabelObject(memberInfo.Name + suffix));
            if(targetType == typeof(string) || targetType == typeof(int) || targetType == typeof(float))
            {
                returnObject.Add(GetIFieldObject(c, memberInfo, subMemberInfo)); 
            }
            else if(targetType == typeof(bool))
            {
                returnObject.Add(GetToggleObject(c, memberInfo, subMemberInfo));
            }
            else if(InspectorSpecifications.IsEnum(targetType))
            {
                returnObject.Add(GetDropdownObject(c, memberInfo, subMemberInfo));
            }
            else
            {
                throw new InvalidOperationException("trying to draw a control for an invalid type - not sure how you got here.");
            }
            return returnObject;
        }

        private GameObject GetDropdownObject(Component C, MemberInfo memberInfo, MemberInfo subMemberInfo = null)
        {
            GameObject contentObject = dropdownPool.GetGameObject();
            DropdownControl dControl = contentObject.GetComponent<DropdownControl>();
            currentContentObjects.Add(dControl);
            dControl.SetTarget(C, memberInfo, subMemberInfo);
            contentObject.SetActive(true);
            return contentObject;
        }

        private GameObject GetToggleObject(Component C, MemberInfo memberInfo, MemberInfo subMemberInfo = null)
        {
            GameObject contentObject = togglePool.GetGameObject();
            ToggleControl tControl = contentObject.GetComponent<ToggleControl>();
            currentContentObjects.Add(tControl);
            tControl.SetTarget(C, memberInfo, subMemberInfo);
            contentObject.SetActive(true);
            return contentObject;
        }

        private GameObject GetIFieldObject(Component C, MemberInfo memberInfo, MemberInfo subMember = null)
        {
            GameObject contentObject = fieldPool.GetGameObject();
            InputFieldControl ifControl = contentObject.GetComponent<InputFieldControl>();
            currentContentObjects.Add(ifControl);
            ifControl.SetTarget(C, memberInfo, subMember);
            contentObject.SetActive(true);
            return contentObject;
        }

        private GameObject GetLabelObject(Component c, MemberInfo memberInfo, string suffix = "", string prefix = "")
        {
            return GetLabelObject(prefix + memberInfo.Name + suffix);
        }

        private GameObject GetLabelObject(string s)
        {
            GameObject retVal = labelPool.GetGameObject();
            retVal.GetComponent<Text>().text = s;
            retVal.SetActive(true);
            return retVal;
        }
        
        public void ClearInspectorControls(List<GameObject> controls)
        {
            foreach(GameObject control in controls)
            {
                PrefabTypeTag tag = control.GetComponent<PrefabTypeTag>();
                if(tag.prefabType == LoadedAssets.AssetEnums.inspectorlabel)
                {
                    labelPool.RecycleObject(control);
                }
                if(tag.prefabType == LoadedAssets.AssetEnums.inspectorinputfield)
                {
                    fieldPool.RecycleObject(control);
                }
                if(tag.prefabType == LoadedAssets.AssetEnums.inspectortoggle)
                {
                    togglePool.RecycleObject(control);
                }
                if(tag.prefabType == LoadedAssets.AssetEnums.inspectordropdown)
                {
                    dropdownPool.RecycleObject(control);
                }
            }
        }

        public void RefreshInspectors()
        {
            foreach(ControlBase cb in currentContentObjects)
            {
                cb.Refresh();
            }
        }
    }
}
