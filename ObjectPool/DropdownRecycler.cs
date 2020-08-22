using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ObjectExplorer
{
    public class DropdownRecycler : IPoolObjectRecycler
    {
        public GameObject RecycleObject(GameObject go)
        {
            go.SetActive(false);
            go.transform.Find("Label").GetComponent<Text>().color = TUNING.CONTROLS.EDITABLE.textColor;
            go.transform.Find("Arrow").GetComponent<Image>().color = TUNING.CONTROLS.EDITABLE.textColor;
            go.GetComponent<Dropdown>().interactable = true;
            go.GetComponent<DropdownControl>().ClearState();
            return go;
        }
    }
}
