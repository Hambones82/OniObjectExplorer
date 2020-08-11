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
        //protected static GameObject panelPrefab;

        protected UIObjectPool uIObjectPool;

        //poniter to the parent object
        protected GameObject panel;
        //pointer to the content window, into which the buttons will go
        protected GameObject panelContent;

        protected List<GameObject> contentObjects = new List<GameObject>();

        protected GameObject parent;

        protected ExplorerManager explorerManager;

        //parent would be passed in, as well as emanager.  ptype is the prefab for the panel itself.   content type is 
        //                                                                                           unnecessary, as it will be specified
        //                                                                                           within the poolobjectcreator
        public ScrollablePanel(GameObject parent, ExplorerManager eManager, LoadedAssets.AssetEnums pType)
        {
            panelType = pType;
            //panelPrefab = LoadedAssets.GetLoadedPrefab(panelType); //probably don't need
            explorerManager = eManager;
            this.parent = parent;
            panel = LoadedAssets.InstantiatePostProcessed(pType, parent.transform);
            panelContent = panel.transform.GetChild(0).GetChild(0).gameObject;
            SetPosition();
        }

        protected virtual void SetPosition() { }

        //not sure about this one...
        public virtual void SetCurrentGO(GameObject go) { }

        public void RemoveObjects()
        {
            foreach (GameObject g in contentObjects)
            {
                uIObjectPool.RecycleObject(g);
            }
            contentObjects.RemoveAll((g) => true);
        }
    }
}
