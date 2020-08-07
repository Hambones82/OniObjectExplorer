using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ObjectExplorer
{
    public class LabelCreator : IPoolObjectCreator
    {
        private LoadedAssets.AssetEnums labelPrefabType;

        public LabelCreator(LoadedAssets.AssetEnums labelType)
        {
            labelPrefabType = labelType;
        }

        public GameObject CreateNewPoolObject()
        {
            GameObject retVal = LoadedAssets.InstantiatePostProcessed(labelPrefabType, Globals.DebugCanvas.transform);
            retVal.SetActive(false);
            return retVal;
        }
    }
}
