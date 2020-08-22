using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ObjectExplorer
{
    public class ToggleRecycler : IPoolObjectRecycler
    {
        public GameObject RecycleObject(GameObject go)
        {
            go.SetActive(false);
            go.transform.Find("Background/Checkmark").GetComponent<Image>().color =
                        TUNING.CONTROLS.EDITABLE.textColor;
            go.GetComponent<Toggle>().interactable = true;
            return go;
        }
    }
}
