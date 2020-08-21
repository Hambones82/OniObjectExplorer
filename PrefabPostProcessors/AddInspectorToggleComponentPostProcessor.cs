using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ObjectExplorer
{
    public class AddInspectorToggleComponentPostProcessor : ILoadedAssetPostProcessor
    {
        public GameObject AssetPostProcess(GameObject go, object parameters = null)
        {
            ToggleHelper helper = go.AddComponent<ToggleHelper>();
            ToggleControl control = go.AddComponent<ToggleControl>();
            control.toggleHelper = helper;
            Toggle toggle = go.GetComponent<Toggle>();
            //toggle.onValueChanged.AddListener(helper.callBacks.Invoke);
            control.toggle = toggle;
            helper.toggleElement = toggle;
            helper.callBacks.AddListener(control.SetValue);
            return go;
        }

        public bool CanProcess(GameObject go)
        {
            if (go.GetComponent<PrefabTypeTag>().prefabType == LoadedAssets.AssetEnums.inspectortoggle)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
