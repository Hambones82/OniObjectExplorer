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

        private static readonly List<Type> UniversalPermittedPropertyTypes = new List<Type>()
        {
            typeof(Vector2),
            typeof(Vector3),
            typeof(Vector4),
            typeof(Color),
            typeof(int),
            typeof(string),
            typeof(float),
            typeof(Text),
            typeof(LocText),
            typeof(bool)
        };

        private static readonly List<Type> basicTypes = new List<Type>()
        {
            typeof(int),
            typeof(string),
            typeof(float),
            typeof(bool)
        };

        /*
        private static readonly List<Type> ExcludedTypes = new List<Type>()
        {
            typeof(GameObject),
            typeof(Transform),
            typeof(Rect)
        };
        */

        private static LoadedAssets.AssetEnums labelType = LoadedAssets.AssetEnums.inspectorlabel;
        private static LoadedAssets.AssetEnums inputFieldType = LoadedAssets.AssetEnums.inspectorinputfield;
        private static LoadedAssets.AssetEnums toggleType = LoadedAssets.AssetEnums.inspectortoggle;

        private UIObjectPool labelPool;
        private UIObjectPool fieldPool;
        private UIObjectPool togglePool;

        public InspectorGenerator(ExplorerManager eManager)
        {
            labelPool = new UIObjectPool(new LabelCreator(labelType), new LabelRecycler());
            fieldPool = new UIObjectPool(new InputFieldCreator(inputFieldType), new InputFieldRecycler());
            togglePool = new UIObjectPool(new ToggleCreator(toggleType, eManager), new ToggleRecycler());
        }
        
        public IEnumerable<List<GameObject>> GetComponentControls(Component c)
        {
            Debug.Log("getting inspectors");
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
                    if (/*!ExcludedTypes.Contains(pType) && */UniversalPermittedPropertyTypes.Contains(pType))
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
                    if (UniversalPermittedPropertyTypes.Contains(fType))
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
            
            if(basicTypes.Contains(memberType))
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
                        if (field.IsStatic || field.IsLiteral || field.IsInitOnly || !basicTypes.Contains(field.FieldType))
                        {
                            continue;
                        }
                        yield return GetObjectList(c, memberInfo, field, field.FieldType, "." + field.Name);
                    }

                    PropertyInfo[] properties = memberType.GetProperties();
                    foreach (PropertyInfo property in properties)
                    {
                        if (!UniversalPermittedPropertyTypes.Contains(property.PropertyType)) continue;
                        if (property.GetIndexParameters().Length != 0) continue;
                        if (!basicTypes.Contains(property.PropertyType)) continue;
                        yield return GetObjectList(c, memberInfo, property, property.PropertyType, "." + property.Name);
                    }
                }  
            }
            yield break;
        }

        private List<GameObject> GetObjectList(Component c, MemberInfo memberInfo, MemberInfo subMemberInfo, Type targetType, string suffix = "")
        {
            List<GameObject> returnObject = new List<GameObject>();
            returnObject.Add(GetLabelObject(memberInfo.Name + suffix));
            //select appropriate function based on targetType
            if(targetType == typeof(string) || targetType == typeof(int) || targetType == typeof(float))
            {
                returnObject.Add(GetIFieldObject(c, memberInfo, subMemberInfo)); //fix this to be generic -- i.e., type determines which fn to call
                                                                                 //might need to pass in that type
            }
            else if(targetType == typeof(bool))
            {
                returnObject.Add(GetToggleObject(c, memberInfo, subMemberInfo));
            }
            else if(targetType.IsEnum)
            {
                //returnObject.Add...
            }
            else
            {
                //this should not happen
            }
            return returnObject;
        }

        //get pulldownobject

        //get toggleobject

        private GameObject GetToggleObject(Component C, MemberInfo memberInfo, MemberInfo subMemberInfo = null)
        {
            GameObject contentObject = togglePool.GetGameObject();
            ToggleControl tControl = contentObject.GetComponent<ToggleControl>();
            tControl.SetTarget(C, memberInfo, subMemberInfo);
            contentObject.SetActive(true);
            return contentObject;
        }

        private GameObject GetIFieldObject(Component C, MemberInfo memberInfo, MemberInfo subMember = null)
        {
            GameObject contentObject = fieldPool.GetGameObject();
            InputFieldControl ifControl = contentObject.GetComponent<InputFieldControl>();
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
        
        //maybe...
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
                    Debug.Log("recycling toggle");
                    togglePool.RecycleObject(control);
                }
            }
        }

        public void RefreshInspectors()
        {
            //throw new NotImplementedException();
        }
    }
}
