using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ObjectExplorer
{
    public class InspectorPanel : ScrollablePanel
    {
        protected static readonly LoadedAssets.AssetEnums holderType = LoadedAssets.AssetEnums.inspectorpanel;
        protected static readonly LoadedAssets.AssetEnums labelType = LoadedAssets.AssetEnums.inspectorlabel;
        protected static readonly LoadedAssets.AssetEnums fieldType = LoadedAssets.AssetEnums.inspectorinputfield;

        private InspectorGenerator inspectorGenerator;

        private Component currentComponent;

        public InspectorPanel(GameObject parent, ExplorerManager eManager)
            :base(parent, eManager, LoadedAssets.AssetEnums.inspectorview)
        {
            uIObjectPool = InspectorPoolFactory.GetInspectorPool(holderType);
            inspectorGenerator = new InspectorGenerator(eManager);
            currentComponent = null;
        }

        public override void RemoveObjects()
        {
            ClearInspectorControls();
            base.RemoveObjects();
        }

        private void ClearInspectorControls()
        {
            List<GameObject> inspectorControls = new List<GameObject>();
            foreach (GameObject contentObject in contentObjects)
            {
                int childNum = contentObject.transform.childCount;
                for(int i = 0; i < childNum; i++)
                {
                    inspectorControls.Add(contentObject.transform.GetChild(i).gameObject); //fix
                }
            }
            inspectorGenerator.ClearInspectorControls(inspectorControls);
        }

        public void SetComponent(Component C)
        {
            currentComponent = C;
            ClearInspectorControls();
            
            foreach(List<GameObject> panelContents in inspectorGenerator.GetComponentControls(C))
            {
                //create a new panel...
                GameObject newPanel = uIObjectPool.GetGameObject();
                contentObjects.Add(newPanel);
                foreach(GameObject go in panelContents)
                {
                    go.transform.SetParent(newPanel.transform);
                }
                newPanel.SetActive(true);
                newPanel.transform.SetParent(panelContent.transform);
                newPanel.transform.SetAsLastSibling();
            }
        }

        protected override void SetPosition()
        {
            panel.GetComponent<RectTransform>().SetLocalPosition(new Vector3(800, 0, 0));
        }
    }
}
