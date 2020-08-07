using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

//probably i'm not disabling the keyboard
//problem is i'm also now not whatevering...
//but we have on end edit going... not sure...
namespace ObjectExplorer
{
    public class UnityFieldAddCallbackComponentPostProcessor : ILoadedAssetPostProcessor
    {
        public GameObject AssetPostProcess(GameObject go)
        {
            InputField[] components = go.GetComponentsInChildren<InputField>();
            //this should only happen once?????
            ///so... problem is... 
            foreach(InputField iF in components)
            {
                Debug.Log("setting if callbacks...");
                InputFieldCallbacks ifCallbacks = iF.gameObject.AddComponent<InputFieldCallbacks>();
                iF.onEndEdit.AddListener((string s) => ifCallbacks.callBacks.Invoke(s));
                iF.onEndEdit.AddListener((string s) => Debug.Log($"num listeners: {iF.onEndEdit.ToString()}"));
                //ok...???
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
