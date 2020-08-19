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
                    foreach (List<GameObject> goList in GetPropertyControls(c, properties[i]))
                    {
                        yield return goList;
                    }
                }
            }
            yield break;
        }

        //the order is wrong... reverse setasfirst / setaslast...
        //we can actually keep much of this... remove the anonymous functions, though.
        //we can even keep "getlabelobject," which just generates a label.

            //still only properties..
        public IEnumerable<List<GameObject>> GetPropertyControls(Component c, PropertyInfo propertyInfo/*, string prefix = ""*/)
        {
            if (!propertyInfo.CanRead) yield break;
            bool canEdit = propertyInfo.CanWrite;

            FieldInfo[] fields = propertyInfo.PropertyType.GetFields();

            if(basicTypes.Contains(propertyInfo.PropertyType))
            {
                List<GameObject> returnObject = new List<GameObject>();
                returnObject.Add(GetLabelObject(propertyInfo.Name));
                returnObject.Add(GetIFieldObject(c, propertyInfo));
                yield return returnObject;
            }
            else
            {
                foreach(FieldInfo field in fields)
                {
                    if(field.IsStatic || field.IsLiteral || field.IsInitOnly)
                    {
                        continue;
                    }
                    List<GameObject> returnObject = new List<GameObject>();
                    returnObject.Add(GetLabelObject(propertyInfo.Name + "." + field.Name));
                    returnObject.Add(GetIFieldObject(c, propertyInfo, field));
                    yield return returnObject;
                }
                        
            }
            yield break;
        }

        private GameObject GetIFieldObject(Component C, PropertyInfo propertyInfo, FieldInfo subField = null)
        {
            GameObject contentObject = fieldPool.GetGameObject();
            InputFieldControl ifControl = contentObject.GetComponent<InputFieldControl>();
            ifControl.SetTarget(C, propertyInfo, subField);
            contentObject.SetActive(true);
            return contentObject;
        }

        private GameObject GetLabelObject(Component c, PropertyInfo propertyInfo, string suffix = "", string prefix = "")
        {
            return GetLabelObject(prefix + propertyInfo.Name + suffix);
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
