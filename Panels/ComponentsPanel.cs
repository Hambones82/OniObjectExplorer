using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ObjectExplorer
{
    public class ComponentsPanel : ButtonPanel
    {
        private const LoadedAssets.AssetEnums panType = LoadedAssets.AssetEnums.componentview;
        private const LoadedAssets.AssetEnums butType = LoadedAssets.AssetEnums.componentbutton;

        public ComponentsPanel(GameObject parent, ExplorerManager eManager) : base(parent, eManager, panType, butType) { }

        protected override void SetPosition()
        {
            panel.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            panel.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            panel.GetComponent<RectTransform>().pivot = new Vector2(0, 0);

            panel.GetComponent<RectTransform>().SetLocalPosition(new Vector3(250, -100, 0));
        }

        public override void SetCurrentGO(GameObject go)
        {
            base.RemoveObjects();
            Component[] components = go.GetComponents(typeof(Component));
            for(int i = 0; i < components.Length; i++)
            {
                int index = i;
                GameObject newButton = uIObjectPool.GetGameObject();
                SetBtnText(newButton, components[i].GetType().ToString());
                SetOnClick(newButton, () => { explorerManager.SetCurrentComponent(components[index]); } );
                newButton.transform.SetParent(panelContent.transform);
                newButton.transform.SetAsFirstSibling();
                contentObjects.Add(newButton);
                newButton.SetActive(true);
            }
        }




    }
}
