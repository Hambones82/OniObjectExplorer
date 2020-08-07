using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ObjectExplorer
{
    public class PathPanel : ButtonPanel
    {
        private const LoadedAssets.AssetEnums panType = LoadedAssets.AssetEnums.pathview;
        private const LoadedAssets.AssetEnums butType = LoadedAssets.AssetEnums.pathbutton;


        public PathPanel(GameObject parent, ExplorerManager eManager) : base(parent, eManager, panType, butType) { }
        

        protected override void SetPosition()
        {
            panel.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            panel.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            panel.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
            
            panel.GetComponent<RectTransform>().SetLocalPosition(new Vector3(-100, 110, 0));
        }

        public override void SetCurrentGO(GameObject go)
        {
            base.RemoveObjects();
            GameObject currentPathTraverse = go;
            while (currentPathTraverse != null)
            {
                GameObject currentGoClosureCopy = currentPathTraverse;
                GameObject newButton = uIObjectPool.GetGameObject();
                newButton.transform.SetParent(panelContent.transform); //must be done in all panels
                SetBtnText(newButton, currentPathTraverse.name + "\\");
                SetOnClick(newButton, () => explorerManager.SetCurrentGameObject(currentGoClosureCopy));
                //at some point, all of the buttons need to be parented to the panel...
                newButton.transform.SetAsFirstSibling();
                contentObjects.Add(newButton);
                newButton.SetActive(true);
                currentPathTraverse = currentPathTraverse?.transform?.parent?.gameObject;
            }
        }
    }
}
