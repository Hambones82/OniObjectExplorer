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
        private ExplorerManager explorerManager;

        public ToggleCreator(LoadedAssets.AssetEnums toggleType, ExplorerManager eManager)
        {
            togglePrefabType = toggleType;
            explorerManager = eManager;
        }

        public GameObject CreateNewPoolObject()
        {
            GameObject retVal = LoadedAssets.InstantiatePostProcessed(togglePrefabType, Globals.DebugCanvas.transform, explorerManager);
            retVal.SetActive(false);
            return retVal;
        }
    }
}
