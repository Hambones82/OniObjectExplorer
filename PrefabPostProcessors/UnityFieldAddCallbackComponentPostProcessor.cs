using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ObjectExplorer
{
    public class UnityFieldAddCallbackComponentPostProcessor : ILoadedAssetPostProcessor
    {
        public GameObject AssetPostProcess(GameObject go, object parameters = null)
        {
            InputField[] components = go.GetComponentsInChildren<InputField>();
            
            foreach(InputField iF in components)
            {
                InputFieldCallbacks ifCallbacks = iF.gameObject.AddComponent<InputFieldCallbacks>();
                iF.onEndEdit.AddListener(ifCallbacks.callBacks.Invoke);
            }
            return go;
        }

        public bool CanProcess(GameObject go)
        {
            Component[] components = go.GetComponentsInChildren<InputField>();
            if (components.Length == 0) return false;
            else return true;
        }
    }
}
