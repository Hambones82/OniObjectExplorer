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
    
    public class DefaultInspectorGenerator : IInspectorGenerator
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
            typeof(LocText)
        };

        private static readonly List<Type> basicTypes = new List<Type>()
        {
            typeof(int),
            typeof(string),
            typeof(float)
        };

        private static readonly List<Type> ExcludedTypes = new List<Type>()
        {
            typeof(GameObject),
            typeof(Transform),
            typeof(Rect)
        };

        private static LoadedAssets.AssetEnums labelType = LoadedAssets.AssetEnums.inspectorlabel;
        private static LoadedAssets.AssetEnums inputFieldType = LoadedAssets.AssetEnums.inspectorinputfield;
        private static LoadedAssets.AssetEnums toggleType = LoadedAssets.AssetEnums.inspectortoggle;

        private UIObjectPool labelPool;
        private UIObjectPool fieldPool;
        private UIObjectPool togglePool;

        public DefaultInspectorGenerator(ExplorerManager eManager)
        {
            labelPool = new UIObjectPool(new LabelCreator(labelType), new LabelRecycler());
            fieldPool = new UIObjectPool(new InputFieldCreator(inputFieldType), new InputFieldRecycler());
            togglePool = new UIObjectPool(new ToggleCreator(toggleType, eManager), new ToggleRecycler());
        }
        
        //this is OK.  we want to call for each property of component c
        //maybe change to returning List<List<GameObject>> but not sure...
        public IEnumerable<List<GameObject>> GetComponentControls(Component c)
        {
            Debug.Log("getting inspectors");
            PropertyInfo[] properties = c.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                Type pType = properties[i].PropertyType;
                if(!ExcludedTypes.Contains(pType) && UniversalPermittedPropertyTypes.Contains(pType))
                {
                    foreach (List<GameObject> goList in GetMemberControls(c, properties[i]))
                    {
                        yield return goList;
                    }
                }
            }
            FieldInfo[] fields = c.GetType().GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                Type fType = fields[i].FieldType;
                if (UniversalPermittedPropertyTypes.Contains(fType))
                {
                    foreach(List<GameObject> goList in GetMemberControls(c, fields[i]))
                    {
                        yield return goList;
                    }

                }
            }
            yield break;
        }

        
            //still only properties..
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
                List<GameObject> returnObject = new List<GameObject>();
                returnObject.Add(GetLabelObject(memberInfo.Name));
                returnObject.Add(GetIFieldObject(c, memberInfo));
                yield return returnObject;
            }
            else
            {
                FieldInfo[] fields = memberType.GetFields();
                foreach (FieldInfo field in fields)
                {
                    if(field.IsStatic || field.IsLiteral || field.IsInitOnly)
                    {
                        continue;
                    }
                    if (!basicTypes.Contains(field.FieldType)) continue;
                    List<GameObject> returnObject = new List<GameObject>();
                    returnObject.Add(GetLabelObject(memberInfo.Name + "." + field.Name));
                    returnObject.Add(GetIFieldObject(c, memberInfo, field));
                    yield return returnObject;
                }
                
                PropertyInfo[] properties = memberType.GetProperties();
                foreach(PropertyInfo property in properties)
                {
                    if (!UniversalPermittedPropertyTypes.Contains(property.PropertyType)) continue;
                    if (property.GetIndexParameters().Length != 0) continue;
                    if (!basicTypes.Contains(property.PropertyType)) continue;
                    List<GameObject> returnObject = new List<GameObject>();
                    returnObject.Add(GetLabelObject(memberInfo.Name + "." + property.Name));
                    returnObject.Add(GetIFieldObject(c, memberInfo, property));
                    yield return returnObject;
                }
                     
            }
            yield break;
        }

        /*
        private GameObject GetContentObject(Component C, PropertyInfo propertyInfo, FieldInfo subField = null)
        {
            GameObject contentObject;
            if()
        }
        */

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

        public enum InspectorType
        {
            field,
            checkbox,
            pulldown
        }
        
    }
}
