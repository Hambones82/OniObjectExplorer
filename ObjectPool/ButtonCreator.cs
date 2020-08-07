using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ObjectExplorer
{
    public class ButtonCreator : IPoolObjectCreator
    {
        private LoadedAssets.AssetEnums buttonPrefabType;
        //private GameObject buttonPrefab;

        public ButtonCreator(LoadedAssets.AssetEnums buttonType)
        {
            buttonPrefabType = buttonType;
            //buttonPrefab = LoadedAssets.GetLoadedPrefab(buttonPrefabType);
        }

        public GameObject CreateNewPoolObject()
        {
            GameObject retVal = LoadedAssets.InstantiatePostProcessed(buttonPrefabType, Globals.DebugCanvas.transform);
            retVal.SetActive(false);
            return retVal;
        }
    }
}
