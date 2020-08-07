using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace ObjectExplorer
{
    public abstract class ButtonPanel : ScrollablePanel
    {
        public ButtonPanel(GameObject parent, ExplorerManager eManager, LoadedAssets.AssetEnums pType, LoadedAssets.AssetEnums bType)
            : base(parent, eManager, pType)
        {
            //the uiobject pool is of button type.  in other words, it is of the type specified in the argument to the constructor
            uIObjectPool = ButtonPoolFactory.GetPathButtonPool(bType);
        }

        protected void SetBtnText(GameObject btn, string text)
        {
            btn.transform.GetChild(0).GetComponent<Text>().text = text;
        }

        protected void SetOnClick(GameObject btn, UnityAction onClick)
        {
            btn.GetComponent<Button>().onClick.AddListener(onClick);
        }
        
    }
}
