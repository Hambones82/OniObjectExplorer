using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ObjectExplorer
{
    public class DropdownCreator : IPoolObjectCreator
    {
        private LoadedAssets.AssetEnums dropdownPrefabType;

        public DropdownCreator(LoadedAssets.AssetEnums dropdownType)
        {
            dropdownPrefabType = dropdownType;
        }

        public GameObject CreateNewPoolObject()
        {
            GameObject retVal = LoadedAssets.InstantiatePostProcessed(dropdownPrefabType, Globals.DebugCanvas.transform);
            retVal.SetActive(false);
            return retVal;
        }
    }
}
