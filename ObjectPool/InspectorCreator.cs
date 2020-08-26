using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ObjectExplorer
{
    class InspectorCreator : IPoolObjectCreator
    {
        private LoadedAssets.AssetEnums inspectorHolderType;
        
        public InspectorCreator(LoadedAssets.AssetEnums holderType)
        {
            inspectorHolderType = holderType;
        }

        public GameObject CreateNewPoolObject()
        {
            GameObject retVal = LoadedAssets.InstantiatePostProcessed(inspectorHolderType, Globals.DebugCanvas.transform);
            retVal.SetActive(false);
            return retVal;
        }
    }
}
