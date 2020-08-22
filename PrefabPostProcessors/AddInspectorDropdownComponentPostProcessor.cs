using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ObjectExplorer 
{
    public class AddInspectorDropdownComponentPostProcessor : ILoadedAssetPostProcessor
    {
        public GameObject AssetPostProcess(GameObject go, object parameters = null)
        {
            DropdownHelper helper = go.AddComponent<DropdownHelper>();
            DropdownControl control = go.AddComponent<DropdownControl>();
            control.dropdownHelper = helper;
            Dropdown dropDown = go.GetComponent<Dropdown>();
            control.dropdown = dropDown;
            helper.dropdown = dropDown;
            helper.callBacks.AddListener(control.SetValue);
            go.GetComponent<DropdownControl>().ClearState();
            return go;

        }

        public bool CanProcess(GameObject go)
        {
            return (go.GetComponent<PrefabTypeTag>().prefabType == LoadedAssets.AssetEnums.inspectordropdown);
        }
    }
}
