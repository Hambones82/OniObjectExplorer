using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ObjectExplorer 
{
    public class AddInspectorFieldComponentPostProcessor : ILoadedAssetPostProcessor
    {
        public GameObject AssetPostProcess(GameObject go, object parameters = null)
        {
            InputFieldControl control = go.AddComponent<InputFieldControl>();
            InputField inputField = go.GetComponent<InputField>();
            inputField.onEndEdit.AddListener((string s) => control.SetValue(s));
            control.inputField = inputField;
            
            return go;
        }

        public bool CanProcess(GameObject go)
        {
            return go.GetComponent<PrefabTypeTag>().prefabType == LoadedAssets.AssetEnums.inspectorinputfield;
        }
    }
}
