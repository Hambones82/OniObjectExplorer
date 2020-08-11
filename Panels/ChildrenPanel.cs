using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ObjectExplorer
{
    public class ChildrenPanel : ButtonPanel
    {
        private const LoadedAssets.AssetEnums panType = LoadedAssets.AssetEnums.scrollview;
        private const LoadedAssets.AssetEnums butType = LoadedAssets.AssetEnums.objectbutton;

        public ChildrenPanel(GameObject parent, ExplorerManager eManager) : base(parent, eManager, panType, butType) { }

        public override void SetCurrentGO(GameObject go)
        {
            RemoveObjects();
            for (int i = 0; i < go.transform.childCount; i++)
            {
                AddChildButton(go.transform.GetChild(i).gameObject);
            }
        }

        public void ShowGOList(List<GameObject> goList)
        {
            RemoveObjects();
            foreach(GameObject go in goList)
            {
                AddChildButton(go);
            }
        }

        private void AddChildButton(GameObject child)
        {
            GameObject childButton = uIObjectPool.GetGameObject();
            SetBtnText(childButton, child.name);
            SetOnClick(childButton, () => explorerManager.SetCurrentGameObject(child));
            childButton.transform.SetParent(panelContent.transform);
            childButton.SetActive(true);
            childButton.transform.SetAsLastSibling();
            contentObjects.Add(childButton);
        }

    }
}
