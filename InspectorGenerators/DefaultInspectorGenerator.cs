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
        
        private static LoadedAssets.AssetEnums labelType = LoadedAssets.AssetEnums.inspectorlabel;
        private static LoadedAssets.AssetEnums inputFieldType = LoadedAssets.AssetEnums.inspectorinputfield;
        private static LoadedAssets.AssetEnums toggleType = LoadedAssets.AssetEnums.inspectortoggle;

        private UIObjectPool labelPool;
        private UIObjectPool fieldPool;
        private UIObjectPool togglePool;

        public DefaultInspectorGenerator()
        {
            labelPool = new UIObjectPool(new LabelCreator(labelType), new LabelRecycler());
            fieldPool = new UIObjectPool(new InputFieldCreator(inputFieldType), new InputFieldRecycler());
            togglePool = new UIObjectPool(new ToggleCreator(toggleType), new ToggleRecycler());
        }
        
        public IEnumerable<List<GameObject>> GetComponentControls(Component c)
        {
            Debug.Log("getting inspectors");
            PropertyInfo[] properties = c.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                foreach(List<GameObject> goList in GetPropertyControls(c, properties[i]))
                {
                    yield return goList;
                }
            }
            yield break;
        }

        //the order is wrong... reverse setasfirst / setaslast...
        public IEnumerable<List<GameObject>> GetPropertyControls(Component c, PropertyInfo propertyInfo, string prefix = "")
        {
            if (!propertyInfo.CanRead) yield break;
            bool canEdit = propertyInfo.CanWrite;
            //Debug.Log("geting property inspector");
            if (propertyInfo.PropertyType == typeof(string))
            {
                List<GameObject> stringList = new List<GameObject>();
                stringList.Add(GetLabelObject(c, propertyInfo, prefix: prefix)); //i guess we can just select the labels ourselves...
                string currentString = (string)(propertyInfo.GetValue(c, null));
                stringList.Add(GetContentObject(currentString, (string s) =>
                {
                    propertyInfo.SetValue(c, s, null);
                },
                InspectorType.field, canEdit));
                yield return stringList;
            }
            else if (propertyInfo.PropertyType == typeof(int))
            {
                List<GameObject> intList = new List<GameObject>();
                intList.Add(GetLabelObject(c, propertyInfo, prefix: prefix));
                int currentInt = (int)(propertyInfo.GetValue(c, null));
                intList.Add(GetContentObject(currentInt.ToString(), (string Int) =>
                {
                    int newInt;
                    if (int.TryParse(Int, out newInt))
                    {
                        propertyInfo.SetValue(c, newInt, null);
                    }
                },
                InspectorType.field, canEdit));
                yield return intList;
            }
            else if (propertyInfo.PropertyType == typeof(float))
            {
                List<GameObject> floatList = new List<GameObject>();
                floatList.Add(GetLabelObject(c, propertyInfo, prefix: prefix));
                float currentFloat = (float)(propertyInfo.GetValue(c, null));
                floatList.Add(GetContentObject(currentFloat.ToString(), (string Float) =>
                {
                    float newFloat;
                    if (float.TryParse(Float, out newFloat))
                    {
                        propertyInfo.SetValue(c, newFloat, null);
                    }
                },
                InspectorType.field, canEdit));
                yield return floatList;
            }
            else if (propertyInfo.PropertyType == typeof(Vector2))
            {
                List<GameObject> vec2xList = new List<GameObject>();
                vec2xList.Add(GetLabelObject(c, propertyInfo, ".x", prefix: prefix));
                Vector2 currentVector2 = (Vector2)(propertyInfo.GetValue(c, null));
                vec2xList.Add(GetContentObject(currentVector2.x.ToString(), (string vec2x) =>
                {
                    Vector2 newVec2x = (Vector2)(propertyInfo.GetValue(c, null));
                    float parseXVal;
                    if(float.TryParse(vec2x, out parseXVal))
                    {
                        propertyInfo.SetValue(c, new Vector2(parseXVal, newVec2x.y), null);
                    }
                },
                InspectorType.field, canEdit));
                yield return vec2xList;
                
                List<GameObject> vec2yList = new List<GameObject>();
                vec2yList.Add(GetLabelObject(c, propertyInfo, ".y", prefix: prefix));
                vec2yList.Add(GetContentObject(currentVector2.y.ToString(), (string vec2y) =>
                {
                    Vector2 newVec2y = (Vector2)(propertyInfo.GetValue(c, null));
                    float parseYVal;
                    if (float.TryParse(vec2y, out parseYVal))
                    {
                        propertyInfo.SetValue(c, new Vector2(newVec2y.x, parseYVal), null);
                    }
                },
                InspectorType.field, canEdit));
                yield return vec2yList;
            }
            //vector 3
            //vector 4
            else if(propertyInfo.PropertyType == typeof(Color))
            {
                Color currentColor = (Color)(propertyInfo.GetValue(c, null));

                List<GameObject> colorRList = new List<GameObject>();
                colorRList.Add(GetLabelObject(c, propertyInfo, ".r", prefix: prefix));
                colorRList.Add(GetContentObject(currentColor.r.ToString(), (string colorR) =>
                {
                    Color newColor = (Color)(propertyInfo.GetValue(c, null));
                    float colorRVal;
                    if(float.TryParse(colorR, out colorRVal))
                    {
                        propertyInfo.SetValue(c, new Color(colorRVal, newColor.g, newColor.b, newColor.a), null);
                    }
                },
                InspectorType.field, canEdit));
                yield return colorRList;

                List<GameObject> colorGList = new List<GameObject>();
                colorGList.Add(GetLabelObject(c, propertyInfo, ".g", prefix: prefix));
                colorGList.Add(GetContentObject(currentColor.g.ToString(), (string colorG) =>
                {
                    Color newColor = (Color)(propertyInfo.GetValue(c, null));
                    float colorGVal;
                    if (float.TryParse(colorG, out colorGVal))
                    {
                        propertyInfo.SetValue(c, new Color(newColor.r, colorGVal, newColor.b, newColor.a), null);
                    }
                },
                InspectorType.field, canEdit));
                yield return colorGList;

                List<GameObject> colorBList = new List<GameObject>();
                colorBList.Add(GetLabelObject(c, propertyInfo, ".b", prefix: prefix));
                colorBList.Add(GetContentObject(currentColor.b.ToString(), (string colorB) =>
                {
                    Color newColor = (Color)(propertyInfo.GetValue(c, null));
                    float colorBVal;
                    if (float.TryParse(colorB, out colorBVal))
                    {
                        propertyInfo.SetValue(c, new Color(newColor.r, newColor.g, colorBVal, newColor.a), null);
                    }
                },
                InspectorType.field, canEdit));
                yield return colorBList;

                List<GameObject> colorAList = new List<GameObject>();
                colorAList.Add(GetLabelObject(c, propertyInfo, ".a", prefix: prefix));
                colorAList.Add(GetContentObject(currentColor.a.ToString(), (string colorA) =>
                {
                    Color newColor = (Color)(propertyInfo.GetValue(c, null));
                    float colorAVal;
                    if (float.TryParse(colorA, out colorAVal))
                    {
                        propertyInfo.SetValue(c, new Color(newColor.r, newColor.g, newColor.b, colorAVal), null);
                    }
                },
                InspectorType.field, canEdit));
                yield return colorAList;
            }
            else if(propertyInfo.PropertyType == typeof(bool))
            {
                List<GameObject> boolList = new List<GameObject>();
                boolList.Add(GetLabelObject(c, propertyInfo, prefix: prefix));
                bool currentBool = (bool)(propertyInfo.GetValue(c, null));
                boolList.Add(GetContentObject(currentBool, (bool Bool) =>
                {
                    propertyInfo.SetValue(c, Bool, null);
                },
                InspectorType.checkbox, canEdit));
                yield return boolList;
            }
            
            else
            {
                /*
                List<GameObject> labelList = new List<GameObject>();
                labelList.Add(GetLabelObject(propertyInfo.PropertyType.ToString()));
                labelList.Add(GetLabelObject(propertyInfo.Name.ToString()));
                yield return labelList;
                */
            }
            yield break;

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

        private GameObject GetContentObject(object currentValue, UnityAction<string> callback, InspectorType inspectorType, bool canEdit)
        {
            GameObject retVal;
            if(inspectorType == InspectorType.field)
            {
                retVal = fieldPool.GetGameObject();
                if (retVal.GetComponent<InputFieldCallbacks>() == null)
                    Debug.Log("if callbacks is null");
                if (retVal.GetComponent<InputFieldCallbacks>().callBacks == null)
                {
                    Debug.Log("component callbacks is null");
                }
                InputField retInputField = retVal.GetComponent<InputField>();
                retInputField.text = currentValue.ToString();
                if(!canEdit)
                {
                    foreach(Text text in retInputField.GetComponentsInChildren<Text>())
                    {
                        text.color = TUNING.CONTROLS.NONEDITABLE.textColor;
                    }
                    retInputField.interactable = false;
                }
                retVal.GetComponent<InputFieldCallbacks>().callBacks.AddListener(callback);
            }
            else
            {
                //throw argumentException
                retVal = fieldPool.GetGameObject();
                
            }
            retVal.SetActive(true);
            return retVal;
        }

        private GameObject GetContentObject(object currentValue, UnityAction<bool> callback, InspectorType inspectorType, bool canEdit)
        {
            GameObject retVal;
            if (inspectorType == InspectorType.checkbox)
            {
                retVal = togglePool.GetGameObject();
                Toggle toggle = retVal.GetComponent<Toggle>();
                toggle.isOn = (bool)currentValue;
                if (!canEdit)
                {
                    retVal.transform.Find("Background/Checkmark").GetComponent<Image>().color =
                        TUNING.CONTROLS.NONEDITABLE.textColor;
                    retVal.GetComponent<Toggle>().interactable = false;
                }
                retVal.GetComponent<Toggle>().onValueChanged.AddListener(callback);
            }
            else
            {
                //throw argumentException
                retVal = togglePool.GetGameObject();
            }
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
