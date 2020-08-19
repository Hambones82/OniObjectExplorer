using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ObjectExplorer
{//remove the explorer manager thing 
    public class InputFieldCreator : IPoolObjectCreator
    {
        private LoadedAssets.AssetEnums fieldPrefabType;
        

        public InputFieldCreator(LoadedAssets.AssetEnums fieldType)
        {
            fieldPrefabType = fieldType;
            
        }

        public GameObject CreateNewPoolObject()
        {
            GameObject retVal = LoadedAssets.InstantiatePostProcessed(fieldPrefabType, Globals.DebugCanvas.transform);
            retVal.SetActive(false);
            return retVal;
        }
    }
}
