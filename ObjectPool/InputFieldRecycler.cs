using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ObjectExplorer
{
    public class InputFieldRecycler : IPoolObjectRecycler
    {

        public GameObject RecycleObject(GameObject go)
        {
            InputField retInputField = go.GetComponent<InputField>();
            foreach (Text text in retInputField.GetComponentsInChildren<Text>())
            {
                text.color = TUNING.CONTROLS.EDITABLE.textColor;
            }
            retInputField.interactable = true;
            go.SetActive(false);
            return go;
        }
    }
}
