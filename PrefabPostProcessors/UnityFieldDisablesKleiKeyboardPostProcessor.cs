using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ObjectExplorer
{
    //this also needs to add the select component thing that disables keyboard on select.
    //i already have the component --it's "disablekleikeyboardonselect"
    public class UnityFieldDisablesKleiKeyboardPostProcessor : ILoadedAssetPostProcessor
    {
        public GameObject AssetPostProcess(GameObject go)
        {
            InputField[] components = go.GetComponentsInChildren<InputField>();
            foreach(InputField iF in components)
            {
                iF.gameObject.AddComponent<DisableKleiKeyboardOnSelect>();
                //need to think carefully about exactly what events trigger this...
                //this might be acceptable behavior for now...
                //iF.onEndEdit.AddListener((string s) => Global.Instance.GetInputManager().GetDefaultController().ToggleKeyboard(false));
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
