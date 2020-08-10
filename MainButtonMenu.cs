using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ObjectExplorer
{
    public class MainButtonMenu
    {
        private GameObject panel;
        private LoadedAssets.AssetEnums buttonType = LoadedAssets.AssetEnums.menubutton;

        public MainButtonMenu(GameObject parent = null)
        {
            panel = LoadedAssets.InstantiatePostProcessed(LoadedAssets.AssetEnums.menubuttonpanel, parent?.transform);
        }

        public void AddButton(string label, UnityAction callback)
        {
            GameObject newButton = LoadedAssets.InstantiatePostProcessed(buttonType, panel.transform);
            newButton.GetComponent<Button>().onClick.AddListener(callback);
            newButton.transform.Find("Text").GetComponent<Text>().text = label;
        }

        public void SetPosition(Vector2 pos)
        {
            RectTransform rectT = panel.GetComponent<RectTransform>();
            rectT.anchorMin = new Vector2(0.5f, 0.5f);
            rectT.anchorMax = new Vector2(0.5f, 0.5f);
            
            panel.transform.SetLocalPosition(pos);
        }

    }
}
