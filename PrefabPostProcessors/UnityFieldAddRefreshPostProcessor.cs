using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ObjectExplorer 
{
    public class UnityFieldAddRefreshPostProcessor : ILoadedAssetPostProcessor
    {
        public GameObject AssetPostProcess(GameObject go, object parameters = null)
        {
            if (parameters.GetType() != typeof(ExplorerManager))
            {
                throw new ArgumentException("post processor must be passed an explorer manager as an argument");
            }
            ExplorerManager explorerManager = (ExplorerManager)parameters;

            InputField[] components = go.GetComponentsInChildren<InputField>();
            
            foreach (InputField iF in components)
            {
                iF.onEndEdit.AddListener((string s) =>
                {
                    Debug.Log("refreshing");
                    explorerManager.Refresh();
                });
            }
            
            return go;
        }

        public bool CanProcess(GameObject go)
        {
            Debug.Log("this processor is working");
            return go.GetComponent<PrefabTypeTag>().prefabType == LoadedAssets.AssetEnums.inspectorinputfield;
        }
    }
}
