using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ObjectExplorer
{
    public class OnMouseoverDisableKleiMousePostProcessor : ILoadedAssetPostProcessor
    {
        public GameObject AssetPostProcess(GameObject go, object parameters = null)
        {
            go.AddComponent<DisableKleiMouseOnMouseover>();
            return go;
        }

        public bool CanProcess(GameObject go)
        {
            bool retVal = false;
            if (go.GetComponentInChildren<ScrollRect>() != null)
            {
                retVal = true;
            }
            else if(go.GetComponent<PrefabTypeTag>().prefabType == LoadedAssets.AssetEnums.menubuttonpanel
                || go.GetComponent<PrefabTypeTag>().prefabType == LoadedAssets.AssetEnums.movehandle)
            {
                retVal = true;
            }
                
            else
            {
                
            }
            return retVal; ;
        }
    }
}
