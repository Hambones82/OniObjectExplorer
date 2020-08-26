using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ObjectExplorer
{
    public class DialogManager
    {
        private GameObject inputFieldDialog;
        public DialogManager()
        {
            inputFieldDialog = LoadedAssets.InstantiatePostProcessed(LoadedAssets.AssetEnums.inputfielddialog, Globals.DebugCanvas.transform);
            
            inputFieldDialog.GetComponent<InternalRefs>()["cancelbutton"].GetComponent<Button>().onClick.AddListener(DeactivateDialog);
            
            inputFieldDialog.SetActive(false);
        }

        public void ActivateDialog(UnityAction<string> callBack, string title = "NONE", string bodyText = "NONE")
        {
            inputFieldDialog.SetActive(true);
            InternalRefs refs = inputFieldDialog.GetComponent<InternalRefs>();
            InputField inputField = refs["input"].GetComponent<InputField>();
            inputField.text = "";
            inputField.onEndEdit.RemoveAllListeners();
            inputField.onEndEdit.AddListener(callBack);
            refs["title"].GetComponent<Text>().text = title;
            refs["message"].GetComponent<Text>().text = bodyText;
        }

        public void DeactivateDialog()
        {
            inputFieldDialog.SetActive(false);

        }
    }
}
