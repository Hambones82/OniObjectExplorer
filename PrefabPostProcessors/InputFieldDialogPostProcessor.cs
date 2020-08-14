using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ObjectExplorer
{
    public class InputFieldDialogPostProcessor : ILoadedAssetPostProcessor
    {
        public InputFieldDialogPostProcessor() { }

        public GameObject AssetPostProcess(GameObject go, object parameters = null)
        {
            GameObject cancelButton = go.transform.GetChild(3).gameObject;
            GameObject title = go.transform.Find("Image/Text").gameObject;
            GameObject inputField = go.transform.GetChild(1).gameObject;
            GameObject messageText = go.transform.GetChild(2).gameObject;


            InternalRefs iRefs = go.AddComponent<InternalRefs>();
            iRefs.RefsList.Add("cancelbutton", cancelButton);
            iRefs.RefsList.Add("title", title);
            iRefs.RefsList.Add("input", inputField);
            iRefs.RefsList.Add("message", messageText);

            return go;

        }

        public bool CanProcess(GameObject go)
        {
            if(go.GetComponent<PrefabTypeTag>().prefabType == LoadedAssets.AssetEnums.inputfielddialog)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
