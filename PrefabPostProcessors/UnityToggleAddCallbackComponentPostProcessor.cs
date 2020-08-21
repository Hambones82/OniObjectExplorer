using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ObjectExplorer
{
    public class UnityToggleAddCallbackComponentPostProcessor : ILoadedAssetPostProcessor
    {
        public GameObject AssetPostProcess(GameObject go, object parameters = null)
        {
            Toggle[] components = go.GetComponentsInChildren<Toggle>();
            if(parameters.GetType() != typeof(ExplorerManager))
            {
                throw new ArgumentException("post processor must be passed an explorer manager as an argument");
            }
            ExplorerManager explorerManager = (ExplorerManager)parameters;
            foreach (Toggle t in components)
            {
                ToggleHelper tCallbacks = t.gameObject.AddComponent<ToggleHelper>();
                t.onValueChanged.AddListener(tCallbacks.callBacks.Invoke);
            }
            return go;
        }

        public bool CanProcess(GameObject go)
        {
            Component[] components = go.GetComponentsInChildren<Toggle>();
            if (components.Length == 0) return false;
            else return true;
        }
    }
}
