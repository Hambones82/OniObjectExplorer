using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ObjectExplorer
{
    public class ToggleCreator : IPoolObjectCreator
    {
        private LoadedAssets.AssetEnums togglePrefabType;

        public ToggleCreator(LoadedAssets.AssetEnums toggleType)
        {
            togglePrefabType = toggleType;
        }

        public GameObject CreateNewPoolObject()
        {
            GameObject retVal = LoadedAssets.InstantiatePostProcessed(togglePrefabType, Globals.DebugCanvas.transform);
            retVal.SetActive(false);
            return retVal;
        }
    }
}
