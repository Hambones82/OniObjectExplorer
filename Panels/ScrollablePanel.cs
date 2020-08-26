using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ObjectExplorer
{
    public abstract class ScrollablePanel
    {   
        protected LoadedAssets.AssetEnums panelType;

        protected UIObjectPool uIObjectPool;
        
        protected GameObject panel;
        protected GameObject panelContent;

        protected List<GameObject> contentObjects = new List<GameObject>();

        protected GameObject parent;

        protected ExplorerManager explorerManager;
        
        public ScrollablePanel(GameObject parent, ExplorerManager eManager, LoadedAssets.AssetEnums pType)
        {
            panelType = pType;
            explorerManager = eManager;
            this.parent = parent;
            panel = LoadedAssets.InstantiatePostProcessed(pType, parent.transform);
            panelContent = panel.transform.GetChild(0).GetChild(0).gameObject;
            SetPosition();
        }

        protected virtual void SetPosition() { }
        
        public virtual void SetCurrentGO(GameObject go) { }

        public virtual void RemoveObjects()
        {
            foreach (GameObject g in contentObjects)
            {
                uIObjectPool.RecycleObject(g);
            }
            contentObjects.RemoveAll((g) => true);
        }
    }
}
