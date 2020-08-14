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
                ToggleCallbacks tCallbacks = t.gameObject.AddComponent<ToggleCallbacks>();
                t.onValueChanged.AddListener((bool b) => tCallbacks.callBacks.Invoke(b));
                //this doesn't work.  i suspect the reason is that refreshing can change the value of other toggles
                //and those toggles will call their onvalue changes, and so on.
                //potentially, look into onpointerclick to see if this issue can be fixed.
                //t.onValueChanged.AddListener((bool b) => explorerManager.Refresh());
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
