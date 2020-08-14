using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ObjectExplorer
{
    public class InputFieldCreator : IPoolObjectCreator
    {
        private LoadedAssets.AssetEnums fieldPrefabType;
        private ExplorerManager eManager;

        public InputFieldCreator(LoadedAssets.AssetEnums fieldType, ExplorerManager eManager)
        {
            fieldPrefabType = fieldType;
            this.eManager = eManager;
        }

        public GameObject CreateNewPoolObject()
        {
            GameObject retVal = LoadedAssets.InstantiatePostProcessed(fieldPrefabType, Globals.DebugCanvas.transform, eManager);
            retVal.SetActive(false);
            return retVal;
        }
    }
}
